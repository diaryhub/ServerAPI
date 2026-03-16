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
    }
}