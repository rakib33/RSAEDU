using Microsoft.AspNet.Identity.EntityFramework;
using RSAEDU.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace RSAEDU.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [StringLength(50)]
        public string UserType { get; set; }//Teacher//Student//Stuff
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("RSAEDU_DBCON")
        {
        }

        public virtual DbSet<ClassInfo> ClassInfoes { get; set; }
        public virtual DbSet<ClassInfoSection> ClassInfoSections { get; set; }
        public virtual DbSet<ExamAttendance> ExamAttendances { get; set; }
        public virtual DbSet<ExamInfo> ExamInfoes { get; set; }
        public virtual DbSet<ExamInfoDetail> ExamInfoDetails { get; set; }
        public virtual DbSet<ExamResult> ExamResults { get; set; }
        public virtual DbSet<ExamTypeInfo> ExamTypeInfos { get; set; }
        public virtual DbSet<FacultyInfo> FacultyInfoes { get; set; }
        public virtual DbSet<FacultyInfoSubject> FacultyInfoSubjects { get; set; }
        public virtual DbSet<GradeInfo> GradeInfoes { get; set; }
        public virtual DbSet<StudentInfo> StudentInfoes { get; set; }
        public virtual DbSet<StudentInfoSubject> StudentInfoSubjects { get; set; }
        public virtual DbSet<SubjectInfo> SubjectInfoes { get; set; }

        public virtual DbSet<UserSubject> UserSubjects { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ClassInfo>().ToTable("ClassInfo");
            modelBuilder.Entity<ClassInfoSection>().ToTable("ClassInfoSection");
            modelBuilder.Entity<ExamAttendance>().ToTable("ExamAttendance");
            modelBuilder.Entity<ExamInfo>().ToTable("ExamInfo");
            modelBuilder.Entity<ExamInfoDetail>().ToTable("ExamInfoDetails");
            modelBuilder.Entity<ExamResult>().ToTable("ExamResult");
            modelBuilder.Entity<ExamTypeInfo>().ToTable("ExamTypeInfo");
            modelBuilder.Entity<FacultyInfo>().ToTable("FacultyInfo");
            modelBuilder.Entity<FacultyInfoSubject>().ToTable("FacultyInfoSubject");
            modelBuilder.Entity<GradeInfo>().ToTable("GradeInfo");
            modelBuilder.Entity<StudentInfo>().ToTable("StudentInfo");
            modelBuilder.Entity<StudentInfoSubject>().ToTable("StudentInfoSubjects");
            modelBuilder.Entity<SubjectInfo>().ToTable("SubjectInfo");
            modelBuilder.Entity<UserSubject>().ToTable("UserSubject");
        }
    }
}