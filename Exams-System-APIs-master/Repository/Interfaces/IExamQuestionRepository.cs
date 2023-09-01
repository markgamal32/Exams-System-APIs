using LMSAPIProject.Models;

namespace LMSAPIProject.Repository.Interfaces
{
    public interface IExamQuestionRepository : IRepository<ExamQuestion>
    {
        public List<ExamQuestion> getByExamId(int examId);
    }
}
