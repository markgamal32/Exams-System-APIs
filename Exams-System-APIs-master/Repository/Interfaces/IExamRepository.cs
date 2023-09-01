using LMSAPIProject.Models;

namespace LMSAPIProject.Repository.Interfaces
{
    public interface IExamRepository : IRepository<Exam>
    {
        public int count();
        public Exam getExam(int id);
        public void Update(Exam exam, IFormCollection form);
        public bool isExists(int id);
    }
}
