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

// 서버 구동 시 DB 테이블 자동 생성 스크립트
using (var scope = app.Services.CreateScope())
{
    // AppDbContext 부분은 본인이 실제 작성한 DbContext 클래스 이름으로 변경해야 합니다.
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
