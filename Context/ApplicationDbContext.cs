using EmployeeManagementSystemLoginHr.Enums;
using EmployeeManagementSystemLoginHr.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Security.Claims;

namespace EmployeeManagementSystemLoginHr.Context
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        private readonly AuditInterceptor _auditInterceptor;
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            AuditInterceptor auditInterceptor
            ) : base(options)
        {
            _auditInterceptor = auditInterceptor;
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.AddInterceptors(_auditInterceptor);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
           .HasIndex(e => e.EmployeeCode)
            .IsUnique();

            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.Email)
            .IsUnique();


            modelBuilder.Entity<Employee>()
            .HasIndex(e => e.NationalId)
            .IsUnique();

            modelBuilder.Entity<Employee>()
                .Property(e => e.NationalId)
                .HasMaxLength(14)
                .IsRequired();

            modelBuilder.Entity<Employee>()
                .Property(e => e.Email)
                .HasMaxLength(100)
            .IsRequired();

            modelBuilder.Entity<Employee>()
                .Property(e => e.Salary)
                .HasColumnType("decimal(18,2)");


            //Seeding Data
            modelBuilder.Entity<Employee>().HasData(
       new Employee
       {
           Id = 1,
           EmployeeCode = "EMP001",
           NationalId = "12345678901234",
           FirstName = "Ahmed",
           LastName = "Ali",
           FirstNameAr = "أحمد",
           LastNameAr = "علي",
           Email = "ahmed.ali@example.com",
           PhoneNumber = "01012345678",
           JobTitle = "Software Engineer",
           JobTitleAr = "مهندس برمجيات",
           Department = "IT",
           DepartmentAr = "تكنولوجيا المعلومات",
           Salary = 15000.00m,
           EmploymentStatus = EmploymentStatus.Active
       },
       new Employee
       {
           Id = 2,
           EmployeeCode = "EMP002",
           NationalId = "22345678901234",
           FirstName = "Sara",
           LastName = "Mohamed",
           FirstNameAr = "سارة",
           LastNameAr = "محمد",
           Email = "sara.mohamed@example.com",
           PhoneNumber = "01098765432",
           JobTitle = "HR Manager",
           JobTitleAr = "مدير الموارد البشرية",
           Department = "HR",
           DepartmentAr = "الموارد البشرية",
           Salary = 18000.00m,
           EmploymentStatus = EmploymentStatus.Active
       },
       new Employee
       {
           Id = 3,
           EmployeeCode = "EMP003",
           NationalId = "32345678901234",
           FirstName = "Omar",
           LastName = "Hassan",
           FirstNameAr = "عمر",
           LastNameAr = "حسن",
           Email = "omar.hassan@example.com",
           PhoneNumber = "01122334455",
           JobTitle = "Marketing Specialist",
           JobTitleAr = "أخصائي تسويق",
           Department = "Marketing",
           DepartmentAr = "التسويق",
           Salary = 12000.00m,
           EmploymentStatus = EmploymentStatus.Inactive
       },
       new Employee
       {
           Id = 4,
           EmployeeCode = "EMP004",
           NationalId = "42345678901234",
           FirstName = "Mona",
           LastName = "Khaled",
           FirstNameAr = "منى",
           LastNameAr = "خالد",
           Email = "mona.khaled@example.com",
           PhoneNumber = "01234567890",
           JobTitle = "Financial Analyst",
           JobTitleAr = "محلل مالي",
           Department = "Finance",
           DepartmentAr = "المالية",
           Salary = 20000.00m,
           EmploymentStatus = EmploymentStatus.Active
       },
       new Employee
       {
           Id = 5,
           EmployeeCode = "EMP005",
           NationalId = "52345678901234",
           FirstName = "Hassan",
           LastName = "Ibrahim",
           FirstNameAr = "حسن",
           LastNameAr = "إبراهيم",
           Email = "hassan.ibrahim@example.com",
           PhoneNumber = "01055667788",
           JobTitle = "System Administrator",
           JobTitleAr = "مسؤول أنظمة",
           Department = "IT",
           DepartmentAr = "تكنولوجيا المعلومات",
           Salary = 14000.00m,
           EmploymentStatus = EmploymentStatus.Active
       }
        );


            //Roles Seeder
            modelBuilder.Entity<IdentityRole>().HasData(new[] {
               new IdentityRole()
               {
                   Id = "7bdb9275-8cd4-4d86-bea6-bbdb5125e28a",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
               },
               new IdentityRole() {
                   Id="7bdb9275-8cd4-4d86-bea6-bbdb5125e28b",
                   Name="User",
                   NormalizedName="USER"
               }
            });

            //Users Seeder

          var admin = new ApplicationUser
            {
                Id = "7bdb9275-8cd4-4d86-bea6-bbdb5125e28a",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@gmail.com",
                NormalizedEmail = "ADMIN@gmail.com",
                EmailConfirmed = true,
                SecurityStamp = string.Empty,
                FirstName = "Admin",
                LastName = "Admin"
            };
            admin.EmailConfirmed = true;
            admin.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(admin, "Admin123");
            admin.SecurityStamp = Guid.NewGuid().ToString();

            var user = new ApplicationUser
            {
                Id = "7bdb9275-8cd4-4d86-bea6-bbdb5125e28b",
                UserName = "userNormal",
                NormalizedUserName = "USERNORMAL",
                Email = "user@gmail.com",
                NormalizedEmail = "USER@GMAIL.COM",
                EmailConfirmed = true,
                SecurityStamp = string.Empty,
                FirstName = "userfirstname",
                LastName = "ulastname"
            };

            user.EmailConfirmed = true;
            user.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(user, "User123");
            user.SecurityStamp = Guid.NewGuid().ToString();

            modelBuilder.Entity<ApplicationUser>().HasData(admin, user);

            //seed users roles

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string> { RoleId = "7bdb9275-8cd4-4d86-bea6-bbdb5125e28b", UserId = "7bdb9275-8cd4-4d86-bea6-bbdb5125e28b" },
            new IdentityUserRole<string> { RoleId = "7bdb9275-8cd4-4d86-bea6-bbdb5125e28a", UserId = "7bdb9275-8cd4-4d86-bea6-bbdb5125e28a" }
                );



            base.OnModelCreating(modelBuilder);

        }


    }

}
