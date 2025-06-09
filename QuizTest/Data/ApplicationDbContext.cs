using Microsoft.EntityFrameworkCore;
using QuizTest.Web.Models.Entities;

namespace QuizTest.Web.ViewData
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        // Example DbSet
        public DbSet<Quizz> Quizzes { get; set; }
        public DbSet<Options> Options { get; set; }
    }
}