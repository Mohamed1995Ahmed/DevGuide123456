using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using DevGuide.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DevGuide.Models.Configrations;
using DevGuide.Models.Models;
using Models.Models;
using Models.Conifgurations;
using Models;


namespace DevGuide.Models
{
    public class ProjectContext : IdentityDbContext<User>
    {
        public ProjectContext(DbContextOptions<ProjectContext> options)
        : base(options)
        {
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply configuration for each class individually
            modelBuilder.ApplyConfiguration(new BadgeConfiguration());
            modelBuilder.ApplyConfiguration(new OptionConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentConfiguration());
            modelBuilder.ApplyConfiguration(new QueryAnswerConfiguration());
            modelBuilder.ApplyConfiguration(new QueryConfiguration());
            modelBuilder.ApplyConfiguration(new QuestionConfiguration());
            modelBuilder.ApplyConfiguration(new QuizConfiguration());
            modelBuilder.ApplyConfiguration(new ReviewConfiguration());
            modelBuilder.ApplyConfiguration(new ScheduleConfiguration());
            modelBuilder.ApplyConfiguration(new SessionConfiguration());

            modelBuilder.ApplyConfiguration(new SkillConfiguration());

            modelBuilder.ApplyConfiguration(new SocialAccountsConfiguration());

            modelBuilder.ApplyConfiguration(new SupportConfiguration());

            modelBuilder.ApplyConfiguration(new UserAnswerConfigration());

            modelBuilder.ApplyConfiguration(new UserBadgesConfiguration());

            modelBuilder.ApplyConfiguration(new UserConfiguration());

            modelBuilder.ApplyConfiguration(new UserQuizConfigration());

            modelBuilder.ApplyConfiguration(new UserSkillsConfiguration());
            modelBuilder.ApplyConfiguration(new ExperienceConfiguration());
            modelBuilder.ApplyConfiguration(new EducationConfiguration());

            modelBuilder.DataSeed();
            
            base.OnModelCreating(modelBuilder);

        }
        public DbSet<Badge> Badge { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<Query> Query { get; set; }
        public DbSet<QueryAnswer> QueryAnswer { get; set; }
        public DbSet<Quiz> Quiz { get; set; }
        public DbSet<User_Answer> User_Answer { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<Review> Review { get; set; }
        public DbSet<Schedule> Schedule { get; set; }
        public DbSet<Session> Session { get; set; }
        public DbSet<Skill> Skill { get; set; }
        public DbSet<Support> Support { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<User_Badges> User_Badges { get; set; }
        public DbSet<User_Skills> User_Skills { get; set; }
        public DbSet<SocialAccounts> User_SocialAccounts { get; set; }

        public DbSet<Option> Option { get; set; }
        public DbSet<User_Quiz> User_Quiz { get; set; }
        public DbSet<Experience> Experience { get; set; }
        public DbSet<Education> Education { get; set; }


    }
}