using Microsoft.EntityFrameworkCore;
using WlidLife.Models;

namespace WlidLife.dData
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Animals> Animals { get; set; }

        public DbSet<Caretaker> Caretaker { get; set; }

        public DbSet<FeedingSchedule> FeedingSchedules { get; set; }
        
        

    }
}
