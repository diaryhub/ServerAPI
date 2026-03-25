using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServerApi.Data;
using System.Text;

using ServerAPI.Services;
using ServerAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5173",
                "http://localhost:5174",
                "https://*.vercel.app")
            .SetIsOriginAllowedToAllowWildcardSubdomains()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// 1. 설정 파일에서 연결 문자열(주소) 가져오기
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. DbContext 등록 (postgreSQL 연결 설정)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")).UseSnakeCaseNamingConvention());

builder.Services.AddScoped<IGachaService, GachaService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILauncherService, LauncherService>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    // appsettings.json의 RedisConnection 값을 읽어와 6379 포트로 연결을 시도합니다.
    options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
    options.InstanceName = "RedisServer";
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
builder.Services.AddAuthorization();

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
app.UseRouting();
app.UseCors("AllowFrontend");

app.UseAuthentication(); // [핵심] 반드시 UseAuthorization() 보다 먼저 호출되어야 합니다.

app.UseAuthorization();

app.MapControllers();

app.Run();
