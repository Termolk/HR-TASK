using HRCRUD.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRCRUD.BD
{
    public class HRSystemContext : DbContext
    {
        public DbSet<Department> Departments { get; set; }
        public DbSet<HR> HRs { get; set; }
        public DbSet<Vacancy> Vacancies { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<TestTask> TestTasks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Application> Applications { get; set; }

        public HRSystemContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // For debug
            //var loggerFactory = LoggerFactory.Create(builder =>
            //{
            //    builder
            //        .AddConsole();
            //});

            optionsBuilder
                //for debug .UseLoggerFactory(loggerFactory)
                .UseSqlite("Data Source=HRSystem.db");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Department  HR
            modelBuilder.Entity<Department>()
                .HasMany(d => d.HRs)
                .WithOne(h => h.Department)
                .HasForeignKey(h => h.DepartmentId);

            // Department Vacancy
            modelBuilder.Entity<Department>()
                .HasMany(d => d.Vacancies)
                .WithOne(v => v.Department)
                .HasForeignKey(v => v.DepartmentId);

            // HR Vacancy
            modelBuilder.Entity<HR>()
                .HasMany(h => h.Vacancies)
                .WithOne(v => v.HR)
                .HasForeignKey(v => v.HRId);

            // Candidate TestTask
            modelBuilder.Entity<Candidate>()
                .HasMany(c => c.TestTasks)
                .WithOne(t => t.Candidate)
                .HasForeignKey(t => t.CandidateId);

            // Candidate Application
            modelBuilder.Entity<Candidate>()
                .HasMany(c => c.Applications)
                .WithOne(a => a.Candidate)
                .HasForeignKey(a => a.CandidateId);

            // Vacancy Application
            modelBuilder.Entity<Vacancy>()
                .HasMany(v => v.Applications)
                .WithOne(a => a.Vacancy)
                .HasForeignKey(a => a.VacancyId);

            // Role User
            modelBuilder.Entity<Role>()
                .HasMany(r => r.Users)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleId);
        }
    }
}
