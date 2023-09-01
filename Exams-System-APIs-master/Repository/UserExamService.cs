using LMSAPIProject.Data;
using LMSAPIProject.Models;
using LMSAPIProject.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMSAPIProject.Repository
{
    public class UserExamService : IUserExamRepository
    {
        private readonly ExamContext context;

        public UserExamService(ExamContext context)
        {
            this.context = context;
        }

        public void Delete(UserExam t)
        {
            context.UserExams.Remove(t);
            context.SaveChanges();
        }

        public List<UserExam> GetAll()
        {
            return context.UserExams.ToList();
        }

        public UserExam GetById(int id)
        {
            return context.UserExams.FirstOrDefault(u => u.Id == id);
        }

        public void Insert(UserExam t)
        {
            context.UserExams.Add(t);
            context.SaveChanges();

        }

        public void Update(UserExam t)
        {
            context.UserExams.Update(t);
            context.SaveChanges();

        }

        public UserExam Find(int userId, int examId)
        {
            return context.UserExams
            .Include(e => e.Exam)
            .FirstOrDefault(e => e.ExamId == examId && e.UserId == userId);
        }


        public void DeleteExam(int examId)
        {
            Exam exam = context.Exams.FirstOrDefault(e => e.Id == examId);
            context.Exams.Remove(exam);
            context.SaveChanges();
        }
    }
}
