
//        public IConfiguration Configuration { get; }


using builder_mgmt_server.Controllers;
using builder_mgmt_server.Database;
using builder_mgmt_server.Models;
using builder_mgmt_server.Models.Account;
using builder_mgmt_server.Models.ChatMessages;
using builder_mgmt_server.Models.Contact;
using builder_mgmt_server.Models.Project;
using builder_mgmt_server.Models.Tasks;
using builder_mgmt_server.Models.TasksBusyness;
using builder_mgmt_server.Models.User;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace builder_mgmt_server
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

            services.AddScoped<IDbOperations, DbOperations>();

            services.AddTransient<ITaskModel, TaskModel>();
            services.AddTransient<ITaskWorkloadModel, TaskWorkloadModel>();
            services.AddTransient<IUserModel, UserModel>();
            services.AddTransient<IAccountModel, AccountModel>();
            services.AddTransient<IProjectModel, ProjectModel>();
            services.AddTransient<ITaskChatMessagesModel, TaskChatMessagesModel>();
            services.AddTransient<IProjectChatMessagesModel, ProjectChatMessagesModel>();
            services.AddTransient<IContactModel, ContactModel>();

            services.AddScoped<IUserIdService, UserIdService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<OptionsMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x
              .AllowAnyMethod()
              .AllowAnyHeader()
              .SetIsOriginAllowed(origin => true) // allow any origin
              .AllowCredentials()); // allow credentials

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}


//https://stackoverflow.com/questions/42199757/enable-options-header-for-cors-on-net-core-web-api
public class OptionsMiddleware
{
    private readonly RequestDelegate _next;

    public OptionsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext context)
    {
        //if (context.Request.Method != "OPTIONS") { await this._next.Invoke(context); }
        return BeginInvoke(context);
    }

    private Task BeginInvoke(HttpContext context)
    {
        if (context.Request.Method == "OPTIONS")
        {
            var acao = new[] { (string)context.Request.Headers["Origin"] };
            context.Response.Headers.Add("Access-Control-Allow-Origin", acao);
            context.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "Origin, X-Requested-With, Content-Type, Accept" });
            context.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "GET, POST, PUT, DELETE, OPTIONS" });
            context.Response.Headers.Add("Access-Control-Allow-Credentials", new[] { "true" });
            context.Response.Headers.Add("Vary", new[] { "origin" });
            context.Response.Headers.Add("Timing-Allow-Origin", acao);

            context.Response.StatusCode = 200;
            return context.Response.WriteAsync("OK");
        }

        return _next.Invoke(context);
    }
}

//app.UseMiddleware<ErrorLoggingMiddleware>();
//    public class ErrorLoggingMiddleware
//    {
//        private readonly RequestDelegate _next;

//        public ErrorLoggingMiddleware(RequestDelegate next)
//        {
//            _next = next;
//        }

//        public async Task Invoke(HttpContext context)
//        {
//            try
//            {
//                await _next(context);
//            }
//            catch (Exception e)
//            {
//                // Use your logging framework here to log the exception 
//                throw;
//            }
//        }
//    }

//}
