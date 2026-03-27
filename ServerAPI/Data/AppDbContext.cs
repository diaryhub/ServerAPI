using Microsoft.EntityFrameworkCore;
using ServerApi.Models;
using ServerAPI.Models;

namespace ServerApi.Data
{
    // 가게 주인(DbContext): DB와의 모든 통신을 담당하는 핵심 클래스
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        // DB의 'Users' 테이블과 매핑되는 진열대 명부
        public DbSet<User> Users { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<UserInventory> UserInventories { get; set; } 
        public DbSet<GachaRate> GachaRates { get; set; } 
        public DbSet<GachaLog> GachaLogs { get; set; }
        public DbSet<GachaBanner> GachaBanners { get; set; }
        public DbSet<Notice> Notices { get; set; }
        public DbSet<GameVersion> GameVersions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 유저별 인벤토리 조회 최적화
            modelBuilder.Entity<UserInventory>()
                .HasIndex(ui => ui.UserId)
                .HasDatabaseName("ix_user_inventories_user_id");

            // 가챠 로그 유저별 조회 최적화
            modelBuilder.Entity<GachaLog>()
                .HasIndex(gl => gl.UserId)
                .HasDatabaseName("ix_gacha_logs_user_id");

            // 이메일 중복 방지 + 로그인 조회 최적화
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique()
                .HasDatabaseName("ix_users_email");
        }
    }
}