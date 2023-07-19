using FileExplorer.DataConnect;
using FileExplorer.DataModel;
using FileExplorer.Repo;
using FileExplorer.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FileExplorer.API
{
    public static class ControllerExtension
    {
        public static void EFCoreService(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<FileExplorerContext>(options =>
            {
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("SqlConn"),
                    d => { d.MigrationsHistoryTable("__EFMigrationsHistory", "FileExplorer"); }
                );
            });

            // User services
            builder.Services.AddScoped<IService<UserModel>, UserService>();
            builder.Services.AddScoped<IRepository<UserModel>, UserRepository>();

            // Storage services
            builder.Services.AddScoped<IService<FileModel>, StorageService>();
            builder.Services.AddScoped<IRepository<FileModel>, StorageRepository>();

            builder.Services.AddAuthorization();

            builder.Services.AddControllers();
        }

        public static void JWTService(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey
                        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });
        }

        public static void CorsService(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
        }

    }
}
