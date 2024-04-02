using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CRUD_NEWS.Models
{
    public class NewsDbContext : DbContext
    {
        public NewsDbContext(DbContextOptions<NewsDbContext> options) :base(options)
        {
        }
        public DbSet<NewsModel> News { get; set; }
    }
}
