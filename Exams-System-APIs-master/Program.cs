using Microsoft.EntityFrameworkCore;
using LMSAPIProject.Data;
using LMSAPIProject.Repository.Interfaces;
using LMSAPIProject.Repository;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace LMSAPIProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

			////////////////////////////////////////////////////// to allow cors 
			var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
			//////////////////////////////////////////////////////

			// Add services to the container.

			builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ExamContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("Exams")));

            builder.Services.AddScoped<IAdminRepository, AdminService>();
            builder.Services.AddScoped<IUserRepository, UserService>();
            builder.Services.AddScoped<IContactUsRepository, ContactUsService>();
            builder.Services.AddScoped<IExamRepository, ExamService>();
            builder.Services.AddScoped<IExamQuestionRepository, ExamQuestionService>();
            builder.Services.AddScoped<IQuestionOptionRepository, QuestionOptionService>();
            builder.Services.AddScoped<IStudentRepository, StudentService>();
            builder.Services.AddScoped<IUserExamRepository, UserExamService>();
            ////////////////////////////////////////////////////// to allow cors 
            builder.Services.AddCors(options =>
			{
				options.AddPolicy(MyAllowSpecificOrigins,
				builder =>
				{
					builder.AllowAnyOrigin();
					builder.AllowAnyMethod();
					builder.AllowAnyHeader();
				});
			});
            //////////////////////////////////////////////////////
            // JWT Validation
            builder.Services.AddAuthentication(options => options.DefaultAuthenticateScheme = "mySchema")
                .AddJwtBearer("mySchema", option =>
                {
                    string sKey = "welcome to my account in angular exam project";
                    var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(sKey));
                    option.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = secretKey,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };
                }
                );


			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
			////////////////////////////////////////////////////// to allow cors 
			app.UseCors(MyAllowSpecificOrigins);
			//////////////////////////////////////////////////////
			app.MapControllers();

            app.Run();
        }
    }
}