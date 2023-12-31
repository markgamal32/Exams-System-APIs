﻿using Azure.Core;
using Microsoft.EntityFrameworkCore;
using LMSAPIProject.Data;
using LMSAPIProject.Models;
using LMSAPIProject.Repository.Interfaces;

namespace LMSAPIProject.Repository
{
    public class ExamService : IExamRepository
    {
        private readonly ExamContext context;

        public ExamService(ExamContext context)
        {
            this.context = context;
        }
        public void Delete(Exam t)
        {
            context.Exams.Remove(t);
            context.SaveChanges();
        }

        public List<Exam> GetAll()
        {
            return context.Exams.ToList();
        }

        public Exam GetById(int id)
        {
            return context.Exams.FirstOrDefault(u => u.Id == id);
        }

        public void Insert(Exam exam)
        {
            context.Exams.Add(exam);
            context.SaveChanges();
            
        }

        public void Update(Exam exam)
        {
            var existingExam = this.getExam(exam.Id);

            existingExam.Name = exam.Name;

            for (int i = 0; i < exam.ExamQuestions.Count; i++)
            {
                var examQuestion = exam.ExamQuestions[i];
                var existingQuestion = existingExam.ExamQuestions.FirstOrDefault(q => q.Id == examQuestion.Id);

                existingQuestion.Title = examQuestion.Title;
                for (int j = 0; j < examQuestion.Options.Count; j++)
                {
                    var examOption = examQuestion.Options[j];
                    var existingOption = existingQuestion.Options.FirstOrDefault(o => o.Id == examOption.Id);

                    existingOption.Title = examOption.Title;
                    existingOption.IsRight = examOption.IsRight;
                }
            }

            context.SaveChanges();
        }

        public void Update(Exam exam , IFormCollection form)
        {
            var existingExam = this.getExam(exam.Id);

            existingExam.Name = exam.Name;

            for (int i = 0; i < exam.ExamQuestions.Count; i++)
            {
                var examQuestion = exam.ExamQuestions[i];
                var existingQuestion = existingExam.ExamQuestions.FirstOrDefault(q => q.Id == examQuestion.Id);

                existingQuestion.Title = examQuestion.Title;
                foreach (var existingOption in existingQuestion.Options)
                {
                    existingOption.Title = examQuestion.Options.FirstOrDefault(o => o.Id == existingOption.Id)?.Title;

                    if (form[$"ExamQuestions[{i}].CorrectOptionId"] == existingOption.Id.ToString())
                    {
                        existingOption.IsRight = true;
                    }
                    else
                    {
                        existingOption.IsRight = false;
                    }
                }
            }

            context.SaveChanges();
        }


        public int count()
        {
            return context.Exams.Count();
        }

        public Exam getExam(int id)
        {
            return context.Exams.Include(e => e.ExamQuestions)
                .ThenInclude(q => q.Options)
                .FirstOrDefault(e => e.Id == id);
        }

        public bool isExists(int id)
        {
            return (context.Exams.Where(u => u.Id == id).Count() > 0) ? true : false;
        }
    }
}
