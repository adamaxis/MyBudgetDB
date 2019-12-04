using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using MyBudgetDB.Authorization;
using MyBudgetDB.Data;
using MyBudgetDB.Services;

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
            
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<BudgetService>();
            
            services.AddScoped<IAuthorizationHandler, IsBudgetOwnerHandler>();

            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
            });

            services.AddAuthorization(options => {
                options.AddPolicy("CanViewBudget",
                    policyBuilder => policyBuilder
                        .AddRequirements(new IsBudgetOwnerRequirement()));
            });

            // set to status code when not authorize in api
            services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    if (context.Request.Path.StartsWithSegments("/api"))
                    {
                        context.Response.StatusCode = 401;
                        return Task.CompletedTask;
                    }
                    return Task.CompletedTask;
                };
            });

            services.AddCors();

            services.AddMvc(options =>
            {
                options.RespectBrowserAcceptHeader = true; 
            });
            
            services.AddMvc(options =>
                {
                    options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                    options.FormatterMappings.SetMediaTypeMappingForFormat
                        ("xml", MediaTypeHeaderValue.Parse("application/xml"));
                    options.FormatterMappings.SetMediaTypeMappingForFormat
                        ("config", MediaTypeHeaderValue.Parse("application/xml"));
                    options.FormatterMappings.SetMediaTypeMappingForFormat
                        ("js", MediaTypeHeaderValue.Parse("application/json"));
                })
                .AddXmlSerializerFormatters();
            
            services.AddAntiforgery(options =>
            {
                //cookie should only be transmitted on HTTPS request
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            services.AddMvcCore()
                .AddFormatterMappings();
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
                app.UseCors(x => x
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseCookiePolicy();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "budget_id",
                    template: "{id}/",
                    defaults: new { controller = "Budget", action = "ViewBudget" }
                );
                routes.MapRoute(
                    name: "delete",
                    template: "delete/{id}",
                    defaults: new { controller = "Budget", action = "DeleteBudget" }
                );
                routes.MapRoute(
                    name: "delete_remove",
                    template: "remove/{id}",
                    defaults: new { controller = "Budget", action = "DeleteBudget" }
                );
                routes.MapRoute(
                    name: "edit",
                    template: "edit/{id}",
                    defaults: new { controller = "Budget", action = "EditBudget" }
                );
                routes.MapRoute(
                    name: "edit_modify",
                    template: "modify/{id}",
                    defaults: new { controller = "Budget", action = "EditBudget" }
                );
                routes.MapRoute(
                    name: "edit_change",
                    template: "change/{id}",
                    defaults: new { controller = "Budget", action = "EditBudget" }
                );
                routes.MapRoute(
                    name: "edit_add",
                    template: "add/{id}",
                    defaults: new { controller = "Budget", action = "EditBudget" }
                );
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
