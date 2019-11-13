using Microsoft.EntityFrameworkCore;

namespace www_td.Database.Models
{
    public class WebApiContext : DbContext
    {
        public WebApiContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<machinestats> machinestats { get; set; }
    }
}