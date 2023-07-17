using Microsoft.EntityFrameworkCore;
using PaelystSolution.Domain.Entities;

namespace PaelystSolution.Infrastructure.ApplicationContext
{
    public class SolutionContext: DbContext
    {
        public SolutionContext(DbContextOptions<SolutionContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Document> Documents { get; set; }
      
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<User>().HasData(
        //        new User
        //        {
        //            UserId = new Guid(),
        //            UserName = "Admin",
        //            UserEmail = "Yusufadetola07@gmail.com",
        //            IsDeleted = false,
        //            CreatedOn = DateTime.UtcNow,
                   
        //        }
        //    );

        //}
    }
}
