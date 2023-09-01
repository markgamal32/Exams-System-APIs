using LMSAPIProject.Models;
using LMSAPIProject.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.Extensions.Options;
using LMSAPIProject.ViewModel;
using System.Runtime.ConstrainedExecution;
using Azure;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LMSAPIProject.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ExamController : ControllerBase
	{
		private IExamRepository _examRepository;
        private IStudentRepository _studentRepo;
        private IUserExamRepository _userExamRepo;
        private readonly IExamQuestionRepository QuestionRepo;
        private readonly IQuestionOptionRepository OptionRepo;

        public ExamController(
            IExamRepository examRepository,
            IStudentRepository studentRepo,
            IUserExamRepository userExamRepo,
            IExamQuestionRepository questionRepo,
            IQuestionOptionRepository optionRepo
        )
        {
            _examRepository = examRepository;
            _studentRepo = studentRepo;
            _userExamRepo = userExamRepo;
            QuestionRepo = questionRepo;
            OptionRepo = optionRepo;
        }



		[HttpGet]
		public IActionResult GetAllExams()
		{
			List<Exam> examModel = _examRepository.GetAll();
			return Ok(examModel);
		}

        [HttpPost]
        public IActionResult Store(AnswersViewModel answers)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // get student
                    User user = _studentRepo.getByEmail(answers.email);
                    if (_examRepository.isExists(answers.examId))
                    {
                        int TotalResult = 0;
                        // get exam
                        Exam exam = _examRepository.getExam(answers.examId);
                        for (int i = 0; i < exam.QuestionCount; i++)
                        {
                            ExamQuestion question = exam.ExamQuestions[i];
                            for (int j = 0; j < 4; j++)
                            {
                                if (question.Options[j].Id == answers.selectedOptions[i])
                                {
                                    if (question.Options[j].IsRight)
                                    {
                                        TotalResult++;
                                    }
                                }
                            }
                        }
                        bool isPassed = (exam.QuestionCount / 2 > TotalResult) ? false : true;

                        UserExam userExam = new UserExam()
                        {
                            UserId = user.Id,
                            ExamId = answers.examId,
                            Degree = TotalResult,
                            IsPassed = isPassed
                        };

                        _userExamRepo.Insert(userExam);
                        return Ok();
                    }
                    return BadRequest();
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpPost("examResult")]
        public IActionResult ExamResult(ResultRequestModel result)
        {
            if (ModelState.IsValid)
            {

                // get result of the exam
                User user = _studentRepo.getByEmail(result.email);
                UserExam examResult = _userExamRepo.Find(user.Id, result.examId);
                if (examResult != null)
                {
                    var reponse = new ResultViewModel()
                    {
                        degree = examResult.Degree,
                        isPassed = examResult.IsPassed,
                        name = examResult.Exam.Name,
                        questionCount = examResult.Exam.QuestionCount
                    };
                    return Ok(reponse);
                }
                 
            }

            return BadRequest();
        }

        [HttpGet("{id:int}")]
        public IActionResult GetExamById(int id)
        {
            var exam = _examRepository.getExam(id);
            if (exam != null)
            {
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                };
                var json = JsonSerializer.Serialize(exam, options);
                return Content(json, "application/json");
            }
            return BadRequest();
        }


        [HttpPost("CheckUserExam")]
        public IActionResult CheckUserExam(ResultRequestModel userExam)
        {
            if (ModelState.IsValid)
            {
                User user = _studentRepo.getByEmail(userExam.email);
                if (_userExamRepo.Find(user.Id, userExam.examId) != null)
                {
                    return BadRequest();
                }
            }
            
            return Ok();
        }
        [HttpPost("storeExam")]
        public IActionResult StoreExam([FromBody] ExamFormViewModel examForm)
        {
            if (ModelState.IsValid)
            {
                Exam exam = new Exam() { Name = examForm.ExamName, QuestionCount = examForm.Questions.Count };
                _examRepository.Insert(exam);

                foreach (QuestionViewModel question in examForm.Questions)
                {
                    ExamQuestion examQuestion = new ExamQuestion()
                    {
                        Title = question.Title,
                        ExamId = exam.Id
                    };

                    QuestionRepo.Insert(examQuestion);

                    foreach (string optionTitle in question.Options)
                    {
                        QuestionOption option = new QuestionOption()
                        {
                            Title = optionTitle,
                            IsRight = question.Options.IndexOf(optionTitle)+1 == question.Checks,
                            ExamQuestionId = examQuestion.Id
                        };

                        OptionRepo.Insert(option);
                    }
                }

                OptionRepo.SaveChanges(); // Save changes to the database

                return Ok(new { message = "Exam created successfully" });
            }

            return BadRequest(ModelState);
        }

        [HttpPost("updateExam")]
        public IActionResult updateExam(Exam exam)
        {
            _examRepository.Update(exam);
            return Ok();
        }

        [HttpDelete("DeleteExam/{examId}")]
        public IActionResult DeleteExam([FromRoute] int examId)
        {
            _userExamRepo.DeleteExam(examId);
            return Ok();
        }




        //// GET: api/<ExamController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //	return new string[] { "value1", "value2" };
        //}

        //// GET api/<ExamController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //	return "value";
        //}

        //// POST api/<ExamController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<ExamController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<ExamController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}


    }
}
