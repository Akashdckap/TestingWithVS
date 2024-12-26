using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using P21_latest_template.Repositories;
using P21_latest_template.Services;
using Serilog;
using Swashbuckle.AspNetCore;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;


namespace P21_latest_template
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
            services.AddMvc(opt =>
            {
                opt.Conventions.Add(new ApiExplorerVisibilityEnabledConvention(Configuration));
            });

            var client = new Client()
            {
                ClientId = Configuration["Client:ClientId"],
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets =
                {
                    new Secret(Configuration["Client:ClientSecret"].Sha256())
                },
                AllowedScopes = { "p21webapi" },
                AccessTokenLifetime = 86400
            };


            services.AddIdentityServer(options => {
                options.IssuerUri = Configuration["IdentityServer:IssuerUri"];
            })
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryClients(new List<Client>() { client })
                .AddInMemoryApiResources(Config.Apis);

            var requiredHttps = false;
            bool.TryParse(Configuration["Jwt:RequiredHttpsMetadata"], out requiredHttps);

            services.AddAuthentication()
              .AddJwtBearer(jwt =>
              {
                  jwt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                  {
                      ValidateIssuer = false
                  };
                  jwt.Authority = Configuration["Jwt:Authority"];//"http://localhost:5000/";//
                  jwt.RequireHttpsMetadata = requiredHttps;
                  jwt.Audience = Configuration["Jwt:Audience"];//"p21webapi"; //

              });



            var licensedClient = Configuration["LicensedClient"];



            var p21connStr = Environment.GetEnvironmentVariable("P21Connection");

            services.AddSingleton<IDbConnectionService>(new DbConnectionService(p21connStr));
            // When a controller or another class requests an IProductService, the DI system will create a new instance of ProductService and inject it into the class.
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IPaymentService, PaymentService>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<IOrderImportService, OrderImportService>();
            services.AddTransient<IAddressRepository, AddressRepository>();
            services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<IContactRepository, ContactRepository>();
            services.AddTransient<ICustDefaultRepository, CustDefaultRepository>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<ICustomerSalesrepRepository, CustomerSalesrepRepository>();
            services.AddTransient<IOeContactCustomerRepository, OeContactCustomerRepository>();
            services.AddTransient<IPriceLibraryXCustXCmpyRepository, PriceLibraryXCustXCmpyRepository>();
            services.AddTransient<IShipToRepository, ShipToRepository>();
            services.AddTransient<IShipToSalesrepRepository, ShipToSalesrepRepository>();
            services.AddTransient<ICounterService, CounterService>();
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<IContactService, ContactService>();
            services.AddTransient<IAddressService, AddressService>();
            services.AddTransient<IPriceLibraryRepository, PriceLibraryRepository>();
            services.AddTransient<ICorpIdRepository, CorpIdRepository>();
            services.AddTransient<IDocumentLinkEntityReqRepository, DocumentLinkEntityReqRepository>();


            if (licensedClient == "MDS")
                services.AddTransient<IProductService, Services.MDS.ProductService>();
            else if (licensedClient == "BTX")
                services.AddTransient<IProductService, Services.BTX.ProductService>();


            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DCKAP P21 Web API", Version = "v1" });
            });
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
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "DCKAP P21 Web API V1");
                c.RoutePrefix = string.Empty;
            });
            app.UseRouting();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseIdentityServer();
            app.UseEndpoints(endPoints =>
            {
                endPoints.MapControllers();
            });
        }
    }
}
