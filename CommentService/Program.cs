using CommentService.Domain;
using CommentService.Domain.Repositories;
using CommentService.Domain.Repositories.Abstract;
using CommentService.Services;
using CommentService.Services.EncryptDecryptData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CommentService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Services.AddDbContext<AppDbContext>(s => s.UseSqlServer(builder.Configuration.GetSection("ConnectionString").Value));
            builder.Services.AddTransient<JWTservice>();
            builder.Configuration.Bind("JWTsettings", new JWTconfig());

            builder.Services.AddAuthentication().AddJwtBearer(option =>
            {
                option.SaveToken = true;
                option.RequireHttpsMetadata = true;
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = JWTconfig.ValidIssuer,
                    ValidateAudience = true,
                    ValidAudience = JWTconfig.ValidAudience,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTconfig.Key)),
                    ValidateIssuerSigningKey = true,
                    SaveSigninToken = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            builder.Services.AddControllers();
            builder.Services.AddScoped<IRoleRepository, RoleRepositoryEF>();
            builder.Services.AddScoped<IUserRepository, UserRepositoryEF>();
            builder.Services.AddScoped<IEncryptDecryptData, EncryptDecryptData>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
 
            app.MapControllers();

            app.Run();
        }
    }
}