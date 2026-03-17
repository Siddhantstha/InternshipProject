using System.Text;
using Application.Interface;
using Application.Kafka.Consumer.ConsumerServices;
using Application.Kafka.Consumer.Kafka_Interface;
using Application.Service;
using Application.Validator;
using Domain.Interface;
using FluentValidation;
using Infrastructure.DBconnect;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using User.api;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<AppDBconnect>(options =>
{
	options.UseNpgsql("Host=localhost;Port=5432;Database=user;Username=postgres;Password=koeJ2449k");
});

// Core Services
builder.Services.ApiDI();
builder.Services.AddControllers();
builder.Services.AddScoped<IUserDetails, UserDetailsService>();
builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();
builder.Services.AddScoped<ValidationActionFilterAttribute>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT Authentication
builder.Services.AddAuthentication("Bearer")
	.AddJwtBearer("Bearer", options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
			ValidAudience = builder.Configuration["JwtSettings:Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!)
			)
		};
	});

builder.Services.AddAuthorization();

// Kafka
builder.Services.AddHostedService<KafkaConsumerService>();
builder.Services.AddScoped<IOrderCreatedHandler, OrderCreatedHandler>();
builder.Services.AddScoped<IUserNotification, NotificationRepository>();

// Build
var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();