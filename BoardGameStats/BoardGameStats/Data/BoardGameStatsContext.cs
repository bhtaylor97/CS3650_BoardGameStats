using BoardGameStats.Models;
using Microsoft.EntityFrameworkCore;

namespace BoardGameStats.Data
{
    public class BoardGameStatsContext : DbContext
    {
        public BoardGameStatsContext(DbContextOptions<BoardGameStatsContext> options)
    : base(options)
        {
        }

        public DbSet<BoardGame> BoardGame { get; set; }
    }
}
