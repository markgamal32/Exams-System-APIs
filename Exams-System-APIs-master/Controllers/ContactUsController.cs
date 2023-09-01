using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LMSAPIProject.Models;
using LMSAPIProject.Repository.Interfaces;


namespace LMSAPIProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactUsController : ControllerBase
    {
        private readonly IContactUsRepository contactUsRepo;

        public ContactUsController(IContactUsRepository contactUsRepo)
        {
            this.contactUsRepo = contactUsRepo;
        }

        [HttpGet]
        public ActionResult<ContactUs> GetContactUsMSGS()
        {
            var contactUsMSGS =  contactUsRepo.GetAll().ToList();
            if(contactUsMSGS == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(contactUsMSGS);
            }
        }
        [HttpGet("{id}")]
        public ActionResult<ContactUs> GetMSGSById(int id)
        {
            var msg = contactUsRepo.GetById(id);
            if(msg == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(msg);
            }
        }
        [HttpPost]
        public ActionResult<ContactUs> AddMSG(ContactUs contactUs)
        {
            if(contactUs == null) { return BadRequest(); }
            if (!ModelState.IsValid) { return BadRequest(); }
            else
            {
                contactUsRepo.Insert(contactUs);
                return CreatedAtAction(nameof(AddMSG), contactUs);
            }
        }
        [HttpDelete("{id}")]
        public ActionResult DeleteMSG(int id)
        {
            var msg = contactUsRepo.GetById(id);
            if(msg == null) { return NotFound(); }
            else 
            {
                contactUsRepo.Delete(msg);
                return Ok(GetContactUsMSGS());
            }
        }
        [HttpPut("{id}")]
        public ActionResult PutMSG(ContactUs contactUs,int id)
        {
            if (contactUs == null) { return BadRequest(); }
            if (!ModelState.IsValid) { return BadRequest(); }
            if (contactUs.Id != id) { return NotFound(); }
            contactUsRepo.Update(contactUs);

            return NoContent();
        }


    }



}