using FPTStella.Application.Common.Interfaces.Persistences;
using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Application.Services;
using FPTStella.Infrastructure.Persistences;
using FPTStella.Infrastructure.UnitOfWorks;

var builder = WebApplication.CreateBuilder(args);

//User Secrets được thêm vào configuration (mặc định trong Development)
builder.Configuration.AddUserSecrets<Program>(optional: true);

// Add services to the container.
builder.Services.AddSingleton<IMongoDbContext, MongoDbContext>();
builder.Services.AddSingleton<IUnitOfWork, UnitOfWork>();

// Đăng ký DI cho Application
builder.Services.AddSingleton<IUserService, UserService>();

builder.Services.AddControllers();
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
