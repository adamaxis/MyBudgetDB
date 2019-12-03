using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
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
            services.AddCors();

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


            /*services.AddAuthorization(options => {
                options.AddPolicy("CanEditPerson",
                    policyBuilder => policyBuilder
                        .AddRequirements(new CanViewBudgetRequirement()));
            });*/
            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
            });

            services.AddMvc(options =>
            {
                options.RespectBrowserAcceptHeader = true; // false by default
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

                //app.UseCors(builder =>
                //{
                //    builder.WithOrigins("https://dmacc.edu",
                //        "http://dmacc.edu",
                //        "https://localhost:44375",
                //        "https://localhost:5001");
                //});
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseCookiePolicy();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "budget",
                    template: "budget/{id}",
                    defaults: new { controller = "Budget", action = "ViewBudget" }
                );
                routes.MapRoute(
                    name: "budget_id",
                    template: "{id}/",
                    defaults: new { controller = "Budget", action = "ViewBudget" }
                );
                routes.MapRoute(
                    name: "view_all",
                    template: "budgets/",
                    defaults: new { controller = "Budget", action = "ViewBudgets" }
                );
                routes.MapRoute(
                    name: "view_all_sin",
                    template: "budget/",
                    defaults: new { controller = "Budget", action = "ViewBudgets" }
                );
                routes.MapRoute(
                    name: "view",
                    template: "view/",
                    defaults: new { controller = "Budget", action = "ViewBudgets" }
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
                    name: "create",
                    template: "create/",
                    defaults: new { controller = "Budget", action = "CreateBudget" }
                );
                routes.MapRoute(
                    name: "new",
                    template: "new/",
                    defaults: new { controller = "Budget", action = "CreateBudget" }
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
