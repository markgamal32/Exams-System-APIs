﻿using LMSAPIProject.Data;
using LMSAPIProject.Models;
using LMSAPIProject.Repository.Interfaces;

namespace LMSAPIProject.Repository
{
    public class StudentService : IStudentRepository
    {
        private readonly ExamContext context;

        public StudentService(ExamContext context)
        {
            this.context = context;
        }

        public void Delete(User t)
        {
            context.Users.Remove(t);
            context.SaveChanges();
        }

        public List<User> GetAll()
        {
            return context.Users.ToList();
        }

        public User GetById(int id)
        {
            return context.Users.FirstOrDefault(u => u.Id == id);
        }

        public void Insert(User t)
        {
            context.Users.Add(t);
            context.SaveChanges();
        }

        public void Update(User t)
        {
            context.Users.Update(t);
            context.SaveChanges();
        }
        public int count()
        {
            return context.Users.Where(u => u.IsAdmin == false).Count();
        }

        public bool isExists(int id)
        {
            return (context.Users.Where(u => u.Id == id).Count() > 0) ? true : false;
        }

        public User getByEmail(string email)
        {
            return context.Users.Where(u => u.Email == email).FirstOrDefault();
        }
    }
}
