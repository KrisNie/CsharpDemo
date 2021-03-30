using System;
using MvcDemo.Components.Mvc;
using MvcDemo.Data.Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MvcDemo.Services.Operation;

namespace MvcDemo.Web
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
            ConfigureMvc(services);
            ConfigureDependencies(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            // StaticFiles default wwwroot
            app.UseStaticFiles();
            // route
            app.UseRouting();
            // Authorization
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // endpoints route
                // /[Controller]/[ActionName]/[Parameters]
                endpoints.MapControllerRoute(
                    name: "Default",
                    pattern: "/{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void ConfigureMvc(IServiceCollection services)
        {
            // Note that run time is not include in this list by default
            // Add the Nuget package Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation
            services
                .AddControllersWithViews()
                .AddRazorRuntimeCompilation();
            services.AddMvc()
                .AddRazorOptions(options => options.ViewLocationExpanders.Add(new ViewLocationExpander()));
        }

        public void ConfigureDependencies(IServiceCollection services)
        {
            services.AddDbContext<LifeContext>(options =>
                options.UseMySQL(Configuration.GetConnectionString("LifeContext")));
            
            services.AddTransient<IOperationTransient, Operation>();
            services.AddScoped<IOperationScoped, Operation>();
            services.AddSingleton<IOperationSingleton, Operation>();
            services.AddSingleton<IOperationSingletonInstance>(new Operation(Guid.Empty));
            services.AddTransient<OperationService, OperationService>();
        }
    }
}