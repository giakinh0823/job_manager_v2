﻿using AutoMapper;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using server;
using server.Config;
using server.Entity;
using server.Middleware;
using Stripe;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
        });


//  config quartz sql server store
builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();
    q.UseSimpleTypeLoader();

    q.UsePersistentStore(x =>
    {
        x.UseProperties = true;
        x.UseClustering();
        // there are other SQL providers supported too 
        x.UseSqlServer(sqlsever =>
        {
            sqlsever.ConnectionString = builder.Configuration.GetConnectionString("SqlConnection"); ;
            sqlsever.TablePrefix = "QRTZ_";
        });
        // this requires Quartz.Serialization.Json NuGet package
        x.UseJsonSerializer();
    });

    q.UseDefaultThreadPool(tp =>
    {
        // Số luồng tối đa
        tp.MaxConcurrency = 10;
    });
});

builder.Services.AddQuartzHostedService(options =>
{
    // when shutting down we want jobs to complete gracefully
    options.WaitForJobsToComplete = true;
});

builder.Services.AddDbContext<JobManagerContext>(options => options.UseSqlServer
    (builder.Configuration.GetConnectionString("SqlConnection")
     ?? throw new InvalidOperationException("Connection string 'SqlConnection' not found.")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRepository<UserRole>, UserRoleRepository>();
builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<ILogRepository, LogRepository>();
builder.Services.AddScoped<IPaymentInfoRepository, PaymentInfoRepository>();


var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

StripeConfiguration.ApiKey = "sk_test_51KDm2BJWdKsH8AduH2n1X1V0wD4v1cNBvgXHPtEMqDNbo7LUvXWEsYTwOUeoiqkRDPAfz8lxTmF6Fb9HjJ8EkIHM00ZmeAIFBB";

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder =>
{
    builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
