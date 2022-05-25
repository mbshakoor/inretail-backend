using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using InRetailDAL.Models;
using Microsoft.EntityFrameworkCore;
using InRetailDAL.Data;
using InRetailDAL.Data.IRepository;
using InRetailDAL.Data.RepositoryImp;
using AutoMapper;
using InRetailDAL.Services.IService;
using InRetailDAL.Services.ServiceImp;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using InRetailDAL.ViewModel;

namespace InRetail
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
            services.AddControllers();

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            //repositories regiteration
            services.AddTransient<IOrganizationRepository, OrganizationRepository>();
            services.AddTransient<IBranchRepository, BranchRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IItemRepository, ItemRepository>();
            services.AddTransient<ISaleOrderRepository, SaleOrderRepository>();
            services.AddTransient<ISaleOrderDetailRepository, SaleOrderDetailRepository>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();

            //services regiteration
            services.AddTransient<IOrganizationService, OrganizationService>();
            services.AddTransient<IBranchService, BranchService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IItemService, ItemService>();
            services.AddTransient<ISaleOrderService, SaleOrderService>();
            services.AddTransient<ICustomerService, CustomerService>();

            services.AddDbContext<InRetailContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DevConnection")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddSwaggerGen();

            services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.IgnoreNullValues = true);

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
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
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
