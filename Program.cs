using AutoMapper.Configuration;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using RestaurantAPI.Authorization;
using RestaurantAPI.Entities;
using RestaurantAPI.Middleware;
using RestaurantAPI.Models.Validators;
using RestaurantAPI.Models;
using RestaurantAPI.Services;
using RestaurantAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Reflection;

var builder = WebApplication.CreateBuilder();

// NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

// Configure service

            var authenticationSettings = new AuthenticationSettings();

            builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);

            builder.Services.AddAuthentication(option=>
            { 
                option.DefaultAuthenticateScheme = "Bearer";
                option.DefaultScheme = "Bearer";
                option.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(cfg=>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = authenticationSettings.JwtIssuer,
                    ValidAudience = authenticationSettings.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey))

                };

            });


            builder.Services.AddAuthorization(options=>
            {
                options.AddPolicy("HasNationality", builder=> builder.RequireClaim("Nationality","German"));
                options.AddPolicy("AtLeast20", builder => builder.AddRequirements(new MinimumAgeRequirement(20)));
                options.AddPolicy("MinNumberOfRestaurant", builder => builder.AddRequirements(new MinimumAmountOfRestaurantsRequirement(2)));
            });
            builder.Services.AddScoped<IAuthorizationHandler,MinimumAgeRequirementHandler>();
            builder.Services.AddScoped<IAuthorizationHandler,ResourceOperationRequirementHandler>();
            builder.Services.AddScoped<IAuthorizationHandler,MinimumAmountOfRestaurantRequirementHandler>();
            builder.Services.AddScoped<IUserContextService,UserContextService>();
            builder.Services.AddControllers().AddFluentValidation()
            builder.Services.AddDbContext<RestaurantDbContext>(option =>{
                option.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });
            builder.Services.AddScoped<RestaurantSeeder>();
            builder.Services.AddSingleton(authenticationSettings);
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
            builder.Services.AddScoped<IRestaurantServices,RestaurantServices>();
            builder.Services.AddScoped<ErrorHandlingMiddleware>();
            builder.Services.AddScoped<RequestTimeMiddleware>();
            builder.Services.AddScoped<IDishService,DishService>();
            builder.Services.AddScoped<IAccountService,AccountService>();
            builder.Services.AddScoped<IPasswordHasher<User>,PasswordHasher<User>>();
            builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDTOValidator>();
            builder.Services.AddScoped<IValidator<RestaurantQuery>,RestaurantQueryValidator>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options => 
            {
                options.AddPolicy("FrontEndClient",poliicyBuilder => 
                { 
                    poliicyBuilder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins(builder.Configuration["AllowedOrigin"]);
                });
            });



var app = builder.Build();
var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<RestaurantSeeder>();
// configure  
app.UseResponseCaching();
app.UseStaticFiles();
app.UseCors("FrontEndClient");
seeder.seed();
/*RestaurantGenerator.Seed(db);*/

if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
}

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestTimeMiddleware>();
app.UseAuthentication();
app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI(c=>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json","restaurant api");
    });

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
endpoints.MapControllers();
});


app.Run();