using LMSAPIProject.Models;

namespace LMSAPIProject.Repository.Interfaces
{
    public interface IContactUsRepository : IRepository<ContactUs>
    {
        
        public int count();
        
    }
}
