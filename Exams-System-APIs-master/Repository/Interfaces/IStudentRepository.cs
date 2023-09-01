using LMSAPIProject.Models;

namespace LMSAPIProject.Repository.Interfaces
{
    public interface IStudentRepository
    {
        public int count();
        public bool isExists(int id);
        public User getByEmail(string email);
    }
}
