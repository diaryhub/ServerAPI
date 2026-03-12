using Microsoft.EntityFrameworkCore;
using ServerApi.Data;

var builder = WebApplication.CreateBuilder(args);
// 1. 설정 파일에서 연결 문자열(주소) 가져오기
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. DbContext 등록 (MySQL 연결 설정)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
