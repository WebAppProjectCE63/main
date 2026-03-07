using Microsoft.EntityFrameworkCore;
using WebApplicationProject.Models; // ดึง Model มาใช้

namespace WebApplicationProject.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<EventParticipation> EventParticipations { get; set; }

        // (ตัวเลือกเสริม) ถ้าอยากให้ UserSettings เข้าไปอยู่ในตาราง Users ด้วย ให้เพิ่ม OnModelCreating แบบนี้ครับ
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // บอกให้ Database รู้ว่า UserSettings ไม่ใช่ตารางใหม่ แต่เป็นคอลัมน์ย่อยของตาราง Users
            modelBuilder.Entity<User>().OwnsOne(u => u.Settings);
        }
    }
}