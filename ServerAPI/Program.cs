using Microsoft.EntityFrameworkCore;
using ServerApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 1. 설정 파일에서 연결 문자열(주소) 가져오기
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. DbContext 등록 (postgreSQL 연결 설정)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")).UseSnakeCaseNamingConvention());

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Game Server API v1");
    // 접속 기본 경로를 swagger로 설정 (필요시)
    c.RoutePrefix = "swagger";
});

// 서버 구동 시 DB 테이블 자동 생성 스크립트
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}
app.UseRouting(); // [추가 권장] 명시적으로 라우팅 활성화

app.UseAuthorization();

app.MapControllers();

app.Run();
