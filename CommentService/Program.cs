using CommentService.Domain;
using CommentService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CommentService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<AppDbContext>(s => s.UseSqlServer(builder.Configuration.GetSection("ConnectionString").Value));
            builder.Configuration.Bind("JWTsetting", new JWTconfig());

            builder.Services.AddAuthentication().AddJwtBearer(option =>
            {
                option.RequireHttpsMetadata = false;
                option.TokenValidationParameters = new TokenValidationParameters() 
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

            builder.Services.AddTransient<JWTservice>();
            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddAutoMapper(typeof(Program).Assembly);

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}