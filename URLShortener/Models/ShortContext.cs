using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace URLShortener.Models
{
    public class ShortContext : DbContext
    {
        public DbSet<URLShort> SUrl { get; set; }
        public ShortContext(DbContextOptions<ShortContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
