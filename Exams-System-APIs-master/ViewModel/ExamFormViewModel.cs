namespace LMSAPIProject.ViewModel
{
    public class ExamFormViewModel
    {   
        public string ExamName { get; set; }
        public List<QuestionViewModel> Questions { get; set; }
    }

    public class QuestionViewModel
    {
        public string Title { get; set; }

        public List<string> Options { get; set; }

        public int Checks { get; set; }
    }
}
