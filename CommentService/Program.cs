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
            builder.Services.AddDbContext<AppDbContext>(option => { 
                option.UseSqlServer(builder.Configuration.GetSection("ConnectionString").Value);
                //option.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
            builder.Services.AddTransient<JWTservice>();
            builder.Configuration.Bind("JWTsettings", new JWTconfig());
            builder.Configuration.Bind("EmailClient", new EmailClient());

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

            //.AddNewtonsoftJson - solution for: System.Text.Json.JsonException: A possible object cycle was detected which is not supported. This can either be due to a cycle or if the object depth is larger than the maximum allowed depth of 32
            builder.Services.AddControllers().AddNewtonsoftJson(option => 
                                              option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            builder.Services.AddScoped<IRoleRepository, RoleRepositoryEF>();
            builder.Services.AddScoped<IUserRepository, UserRepositoryEF>();
            builder.Services.AddTransient<ICommentRepository, CommentRepositoryEF>();
            builder.Services.AddScoped<IEncryptDecryptData, EncryptDecryptData>();
            builder.Services.AddScoped<IActionRepository, ActionRepositoryEF>();
            builder.Services.AddScoped<ListErrors>();
            builder.Services.AddScoped<EmailSender>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseCors(policy => policy.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());

            app.UseAuthentication();
            app.UseAuthorization();
 
            app.MapControllers();

            app.Run();
        }
    }
}