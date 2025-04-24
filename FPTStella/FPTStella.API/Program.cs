using FPTStella.Application.Common.Interfaces.Google;
using FPTStella.Application.Common.Interfaces.Jwt;
using FPTStella.Application.Common.Interfaces.Persistences;
using FPTStella.Application.Common.Interfaces.Repositories;
using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Application.Services;
using FPTStella.Application.UseCases.Auth;
using FPTStella.Contracts.DTOs.Jwt;
using FPTStella.Infrastructure.Data;
using FPTStella.Infrastructure.Persistences;
using FPTStella.Infrastructure.UnitOfWorks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

//User Secrets được thêm vào configuration (mặc định trong Development)
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddHttpClient();
// Đăng ký MongoDbContext và IMongoDatabase
builder.Services.AddSingleton<IMongoDbContext, MongoDbContext>();
builder.Services.AddSingleton(sp => sp.GetRequiredService<IMongoDbContext>().Database);
// Đăng ký các repository và unit of work
builder.Services.AddSingleton<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<IAccountRepository, AccountRepository>();
builder.Services.AddSingleton<IStudentRepository, StudentRepository>();
builder.Services.AddSingleton<IMajorRepository, MajorRepository>();
builder.Services.AddSingleton<IProgramRepository, ProgramRepository>();
builder.Services.AddSingleton<ICurriculumRepository, CurriculumRepository>();
builder.Services.AddSingleton<ISubjectRepository, SubjectRepository>();
builder.Services.AddSingleton<IPO_PLO_MappingRepository, PO_PLO_MappingRepository>();
builder.Services.AddSingleton<IPLORepository, PLORepository>();
builder.Services.AddSingleton<IPORepository, PORepository>();

// Đăng ký DI cho Application
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddSingleton<IStudentService, StudentService>();
builder.Services.AddSingleton<IMajorService, MajorService>();
builder.Services.AddSingleton<IProgramService, ProgramService>();
builder.Services.AddSingleton<ICurriculumService, CurriculumService>();
builder.Services.AddSingleton<ISubjectService, SubjectService>();
builder.Services.AddSingleton<IPO_PLO_MappingService, PO_PLO_MappingService>();
builder.Services.AddSingleton<IPOService, POService>();
builder.Services.AddSingleton<IPO_PLO_MappingService, PO_PLO_MappingService>();
builder.Services.AddSingleton<IPLOService, PLOService>();

builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();
builder.Services.AddScoped<GoogleLoginUseCase>();
builder.Services.AddScoped<IJwtService, JwtService>();


//JWT
var jwtSecret = builder.Configuration["JwtSettings:AccessSecretToken"];
var jwtIssuer = builder.Configuration["JwtSettings:Issuer"];
var jwtAudience = builder.Configuration["JwtSettings:Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer, // "Jwt"
            ValidAudience = jwtAudience, // "Jwt"
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)) // Sử dụng JwtSettings:Secret
        };
    });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Aventa", Version = "v1" });

    // Cấu hình Bearer token 
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Vui lòng nhập 'Bearer' [space] và token vào đây.",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"

    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});
builder.Services.AddAuthorization();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();


app.Run();
