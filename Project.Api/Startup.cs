using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Project.Api.Extensions;
using Project.Api.Hubs;
using Project.BLL.Services;
using Project.Core.Helpers;
using Project.Core.Models.Auth;
using Project.Core.OperationInterfaces;
using Project.Core.RepositoryInterfaces;
using Project.Core.ServiceInterfaces;
using Project.DAL;
using Project.DAL.Repositories;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using System.Reflection;

namespace Project.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSingleton(provider =>
                                         new MapperConfiguration(expression => expression.AddProfile(new MapProfile(Configuration))).CreateMapper());
            var tokenAuthConfiguration = Configuration.GetSection("TokenAuthentication").Get<TokenAuthenticationConfiguration>();

            services.AddAuth(tokenAuthConfiguration, AuthenticationValidation.GetIdentityByLoginPair, AuthenticationValidation.GetIdentityByApiKey);

            services.AddDbContext<ProjectDbContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("DBConnectionString"));
            });
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.WithOrigins("http://127.0.0.1:4200", "http://localhost:4200", "http://Project.primorsky.ru", "http://Project-api.primorsky.ru", "https://Project.primorsky.ru", "https://Project-api.primorsky.ru")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
            services.AddSignalR();
            services.AddControllers();
            services.AddApiVersioning();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Project API",
                    Version = "v1"
                });
                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;

                    var versions = methodInfo.DeclaringType
                        .GetCustomAttributes(true)
                        .OfType<ApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions);

                    return versions.Any(v => $"v{v}" == docName);
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                   {
                     new OpenApiSecurityScheme
                     {
                       Reference = new OpenApiReference
                       {
                         Type = ReferenceType.SecurityScheme,
                         Id = "Bearer"
                       }
                      },
                      System.Array.Empty<string>()
                    }
                  });
            });
            ConfigureEnvServices(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }



            //app.UseHttpsRedirection();
            //  loggerFactory.CreateLogger();
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<MessageHub>("/message-hub");
                endpoints.MapControllers();

            });

        }
        public void ConfigureEnvServices(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork>(provider =>
                new UnitOfWork(provider.GetService<ProjectDbContext>()));

            services.AddScoped<IUserService>(provider =>
                new UserService(provider.GetService<IUnitOfWork>()));

            services.AddScoped<IProgramsService>(
               provider => new ProgramsService(provider.GetService<IUnitOfWork>(), Configuration)
           );

            services.AddScoped<IItemService>(provider =>
               new ItemService(provider.GetService<IUnitOfWork>(), Configuration));

        }
    }
}
