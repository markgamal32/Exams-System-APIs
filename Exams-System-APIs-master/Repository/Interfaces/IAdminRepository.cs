using LMSAPIProject.Models;

namespace LMSAPIProject.Repository.Interfaces
{
    public interface IAdminRepository : IRepository<User>
    {
        bool Find(string email, string password);
        User GetAdmin(string email,string password);
        
    }
}
