using LMSAPIProject.Models;

namespace LMSAPIProject.Repository.Interfaces
{
    public interface IQuestionOptionRepository : IRepository<QuestionOption>
    {
        public List<QuestionOption> getForQuestionsList(List<ExamQuestion> questions);
        public void SaveChanges();
    }
}
