using Microsoft.EntityFrameworkCore;
using www_td.Models;

namespace www_td
{
    public class WebApiContext : DbContext
    {
        public WebApiContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<machinestats> machinestats { get; set; }
    }
}