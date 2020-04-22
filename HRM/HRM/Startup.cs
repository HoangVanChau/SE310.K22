using System;
using System.Text;
using HRM.Helpers;
using HRM.Repositories.Counter;
using HRM.Repositories.Image;
using HRM.Repositories.Team;
using HRM.Repositories.User;
using HRM.Services.Auth;
using HRM.Services.MongoDB;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HRM
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();
            
            //Config ....
            services.Configure<MongoDbSetting>(options =>
            {
                options.ConnectionString = Configuration.GetSection("MongoDb:ConnectionString").Value;
                options.Database = Configuration.GetSection("MongoDb:Database").Value;
                options.StorageDatabase = Configuration.GetSection("MongoDb:StorageDatabase").Value;
                options.UserName = Configuration.GetSection("MongoDb:UserName").Value;
                options.Host = Configuration.GetSection("MongoDb:Host").Value;
                options.Port = Int32.Parse(Configuration.GetSection("MongoDb:Port").Value);
                options.Password = Configuration.GetSection("MongoDb:Password").Value;
                options.AuthMechanism = Configuration.GetSection("MongoDb:AuthMechanism").Value;
            });

            services.Configure<AuthSetting>(opt =>
            {
                opt.SecretCode = Configuration.GetSection("Auth:SecretCode").Value;
                opt.HashCode = Configuration.GetSection("Auth:HashCode").Value;
                opt.AccessTokenExpire = Int32.Parse(Configuration.GetSection("Auth:AccessTokenExpire").Value);
                opt.RefreshTokenExpire = Int32.Parse(Configuration.GetSection("Auth:RefreshTokenExpire").Value);
            });

            // configure jwt authentication
            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("Auth:SecretCode").Value);
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddSingleton<IMongoDbSetting>(sp =>
                sp.GetRequiredService<IOptions<MongoDbSetting>>().Value);
            
            services.AddSingleton<IAuthSetting>(sp =>
                sp.GetRequiredService<IOptions<AuthSetting>>().Value);

            //Mongo DB Service
            services.AddSingleton<MongoDbService>();
            
            //Auth JWT
            services.AddSingleton<IAuthService, AuthServiceImpl>();

            //Repository ....
            services.AddSingleton<IUserRepository, UserRepositoryImpl>();
            services.AddSingleton<IImageRepository, ImageRepositoryImpl>();
            services.AddSingleton<ICounterRepository, CounterRepositoryImpl>();
            services.AddSingleton<ITeamRepository, TeamRepositoryImpl>();
        }

            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseRouting();
            
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}