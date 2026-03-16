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
        public DbSet<Item> Items { get; set; } // 새로 추가
        public DbSet<UserInventory> UserInventories { get; set; } // 새로 추가

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Item>().HasData(
                new Item { Id = 1, Name = "초보자용 검", ItemType = "Weapon", Description = "기본 지급되는 낡은 검입니다." },
                new Item { Id = 2, Name = "회복 포션", ItemType = "Consumable", Description = "체력을 50 회복합니다." },
                new Item { Id = 3, Name = "특별 채용 티켓", ItemType = "Currency", Description = "새로운 캐릭터를 모집할 수 있는 티켓입니다." }
            );
        }
    }
}