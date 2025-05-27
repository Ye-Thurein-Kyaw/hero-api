using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using heroApi.Models;

namespace heroApi.Models
{
    public class HeroContext : DbContext
    {
        public HeroContext(DbContextOptions<HeroContext> options)
            : base(options)
        {
        }

        public DbSet<HeroItem> HeroItems { get; set; } = null!;
        public DbSet<TodoItem> TodoItems { get; set; } = null!;
        
    }
}