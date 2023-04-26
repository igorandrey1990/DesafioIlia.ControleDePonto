using DesafioIlha.ControleDePonto.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioIlha.ControleDePonto.DAL
{
    public class MomentoContext : DbContext
    {
        public MomentoContext(DbContextOptions<MomentoContext> options) : base(options) { }

        public DbSet<Momento> Momentos { get; set; } = null!;
    }
}
