using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eshoponline.Infrastructure;
using eshoponline.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Microsoft.OpenApi.Models;
using MediatR;
using System.Reflection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using eshoponline.Infrastructure.Error;

namespace eshoponline
{
    public class Startup
    {
        public const string DEFAULT_DATABASE_CONNECTIONSTRING = "Filename=EshoponlineContext.db";
        public const string DEFAULT_DATABASE_PROVIDER = "sqlite";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetValue<string>("ConnectionString") ??
                       DEFAULT_DATABASE_CONNECTIONSTRING;


            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddDbContext<EshoponlineContext>(options => options.UseSqlite(connectionString));
            services.AddControllers()
            .AddFluentValidation(opt =>
            {
                opt.RegisterValidatorsFromAssemblyContaining<eshoponline.Controllers.Products.Create>();
            });

            services.AddCors();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });

            services.AddSwaggerGen(x =>
            {
                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT"
                });

                x.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {   new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    new string[] {}}
                });

                x.SwaggerDoc("v1", new OpenApiInfo { Title = "eShop Online API", Version = "v1" });
                x.CustomSchemaIds(y => y.FullName);
            });
            

            services.AddAutoMapper(GetType().Assembly);

            services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseHttpsRedirection();

            
            app.UseCors(x => x.AllowAnyMethod()
                              .AllowAnyHeader()
                              .AllowCredentials());

            
            app.UseAuthentication();
            app.UseAuthorization();
            


            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "eShop Online API V1");
                x.RoutePrefix = string.Empty;
            });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<EshoponlineContext>();
            context.Database.EnsureCreated();

            //using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            //{
            //    var context = serviceScope.ServiceProvider.GetRequiredService<EshoponlineContext>();
            //    context.Database.EnsureCreated();

            //    Address address = new Address()
            //    {
            //        FirstName = "",
            //        LastName = "",
            //        Street = "24 Ch. du Labrador",
            //        Street2 = "",
            //        City = "Paris",
            //        PostalCode = "75000",
            //        Country = "France",
            //    };
            //    context.Address.Add(address);
            //    context.SaveChanges();

            //    User user1 = new User()
            //    {
            //        Email = "basica0@hotmail.com",
            //        FirstName = "Toto",
            //        LastName = "tata",
            //        PhoneNumber = "0645465456",
            //        IsAdmin = true,
            //        IsSuperAdmin = true,
            //        AddressId = address.AddressId
            //    };
            //    context.Users.Add(user1);

            //    User user2 = new User()
            //    {
            //        Email = "basica1@hotmail.com",
            //        FirstName = "Toto",
            //        LastName = "tata",
            //        PhoneNumber = "0645465456",
            //        IsAdmin = true,
            //        IsSuperAdmin = true,
            //        AddressId = address.AddressId
            //    };
            //    context.Users.Add(user2);
            //    context.SaveChanges();

            //    Category category = new Category()
            //    {
            //        Description = "tous les vélos",
            //        Slug = "velo",
            //        Name = "Vélos",
            //    };
            //    context.Categories.Add(category);

            //    Category category2 = new Category()
            //    {
            //        Description = "tous les planches",
            //        Slug = "velo",
            //        Name = "planche",
            //    };
            //    context.Categories.Add(category2);
            //    context.SaveChanges();

            //    Brand brand1 = new Brand()
            //    {
            //        Name = "Mercedes",
            //        LinkName = "mercos"
            //    };
            //    context.Brands.Add(brand1);
            //    context.SaveChanges();

            //    Product product = new Product()
            //    {
            //        Name = "Velo numéro 1",
            //        MainCategoryId = category.CategoryId,
            //        Summary = "un super vélo",
            //        Description = "un super vélo electrique!",
            //        Slug = "velo_numero_1",
            //        Star = -1,
            //        UnitPrice = 200,
            //        UnitsInStock = 100,
            //        BrandId = brand1.BrandId
            //    };
            //    context.Products.Add(product);
            //    context.SaveChanges();

            //    ProductTag tag1 = new ProductTag()
            //    {
            //        ProductId = product.ProductId,
            //        Value = "electrique"
            //    };
            //    context.ProductTags.Add(tag1);
            //    ProductTag tag2 = new ProductTag()
            //    {
            //        ProductId = product.ProductId,
            //        Value = "roues"
            //    };
            //    context.ProductTags.Add(tag2);

            //    context.SaveChanges();
            //}
        }
    }
}
