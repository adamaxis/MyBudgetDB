using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyBudgetDB.Data;
using MyBudgetDB.Services;
using Microsoft.AspNetCore.Authorization;
using MyBudgetDB.Authorization;

namespace MyBudgetDB
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
            var config = new AppSecrets();
            Configuration.Bind("MyBudgetDB", config);
            services.AddSingleton(config);
            
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    $"{config.Database};User ID={config.User};Password={config.Password};{config.Options};"));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<BudgetService>();
            services.AddScoped<IAuthorizationHandler, CanManageBudgetHandler>();

            services.AddAuthorization(options => {
                options.AddPolicy("CanManageBudget", policyBuilder => policyBuilder
                    .AddRequirements(new CanManageBudgetRequirement()));
            });

            /*services.AddAuthorization(options => {
                options.AddPolicy("CanEditPerson",
                    policyBuilder => policyBuilder
                        .AddRequirements(new CanViewBudgetRequirement()));
            });*/
            //services.AddCors();
            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
            });

            services.AddMvc(options =>
            {
                options.RespectBrowserAcceptHeader = true; // false by default
            });

            services.AddMvc()
                .AddXmlSerializerFormatters();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Budget/Error");
                app.UseHsts();
                app.UseCors(builder =>
                {
                    builder.WithOrigins("https://dmacc.edu",
                        "http://dmacc.edu",
                        "https://localhost:44375",
                        "https://localhost:5001");
                });
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseCookiePolicy();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
