global using FundRaisingServer.Models;
using FundRaisingServer.Repositories;
using FundRaisingServer.Services;
using FundRaisingServer.Services.PasswordHashing;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// getting the connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IArgon2Hasher, Argon2Hasher>();
builder.Services.AddScoped<IUserRepository, UserService>();
builder.Services.AddScoped<IPasswordRepository, PasswordService>();

// adding the db context
builder.Services.AddDbContext<FundRaisingDbContext>(options => 
    options
        .UseSqlServer(connectionString: connectionString)
        .LogTo(Console.WriteLine, LogLevel.Information)
    );


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
