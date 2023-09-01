using LMSAPIProject.Models;

namespace LMSAPIProject.Repository.Interfaces
{
    public interface IUserExamRepository : IRepository<UserExam>
    {
        public UserExam Find(int userId, int examId);

        public void DeleteExam(int examId);

	}
}
