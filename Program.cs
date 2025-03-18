
using EmployeeManagementSystemLoginHr.Context;
using EmployeeManagementSystemLoginHr.Dtos;
using EmployeeManagementSystemLoginHr.Models;
using EmployeeManagementSystemLoginHr.Services;
using EmployeeManagementSystemLoginHr.UOW;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using EmployeeManagementSystemLoginHr.Helpers;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using EmployeeManagementSystemLoginHr.Authorization;


namespace EmployeeManagementSystemLoginHr
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers(

                options => 
                {

                    options.Filters.Add<PermissionsBasedAuthorizationFilter>();
                    
                }
                );

            builder.Services.AddValidatorsFromAssemblyContaining<CreateEmployeeValidator>(includeInternalTypes: true);
            //Add Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
             .AddDefaultTokenProviders();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
                
            });
            //Add JWT
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
                };
            });


            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
                options.AddPolicy("AssignUserRole", policy => policy.RequireClaim(Permissions.PermissionClaimType, Permissions.AssignRolePermission));
                options.AddPolicy("ManagePermissions", policy => policy.RequireClaim(Permissions.PermissionClaimType, Permissions.ManagePermissionsPermission));

            });

            //AutoMapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //Add services and UOW
            builder.Services.AddScoped<UnitOfWork>();
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IAuditService, AuditService>();
            //Add Interceptors
            builder.Services.AddScoped<AuditInterceptor>();
            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' followed by a space and your token."
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {} // Empty array means it's applied globally
        }
    });

            });

            var app = builder.Build();
            // Configure the HTTP request pipeline.
            //Add Authentication and Authorization
            app.UseAuthentication();
            app.UseAuthorization();



            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseHttpsRedirection();

            app.MapControllers();

            
            using (var scope = app.Services.CreateScope())
            {
                var services = app.Services.CreateScope().ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();

                try
                {

                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    await UserSeeder.SeedUserAsync(userManager);
                    await UserSeeder.SeedAdminUserAsync(userManager, roleManager);

                    logger.LogInformation("Data seeded");
                    logger.LogInformation("Application Started");
                }
                catch (System.Exception ex)
                {
                    logger.LogWarning(ex, "An error occurred while seeding data");
                }
            }
        

            app.Run();
        }
    }
}
