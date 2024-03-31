global using FundRaisingServer.Models;
using System.Text;
using FundRaisingServer.Configurations;
using FundRaisingServer.Repositories;
using FundRaisingServer.Services;
using FundRaisingServer.Services.PasswordHashing;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// configuring the JwtConfig so that I can use it via DI

// getting the connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");

// Add services to the container.

builder.Services.AddControllers();

// ADDING DIs
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));
builder.Services.AddScoped<IArgon2Hasher, Argon2Hasher>();
builder.Services.AddScoped<IUserRepository, UserService>();
builder.Services.AddScoped<IPasswordRepository, PasswordService>();
builder.Services.AddScoped<IUserAuthLogRepository, UserAuthLogService>();
builder.Services.AddScoped<IJwtTokenRepository, JwtTokenService>();
builder.Services.AddScoped<ILoginRepository, LoginService>();
builder.Services.AddScoped<IUserTypeRepository, UserTypeService>();

// adding the db context
builder.Services.AddDbContext<FundRaisingDbContext>(options => 
    options
        .UseSqlServer(connectionString: connectionString)
        .LogTo(Console.WriteLine, LogLevel.Information)
    );

// adding cors for our cors
builder.Services.AddCors(builder =>
{
    builder.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// adding JWT Auth
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(jwt =>
{
    var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JwtConfig:Secret").Value!);
    jwt.SaveToken = true;

    jwt.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false,
        RequireExpirationTime = false,
    };
});

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

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
