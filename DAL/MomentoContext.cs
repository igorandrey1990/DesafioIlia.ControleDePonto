using DesafioIlha.ControleDePonto.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioIlha.ControleDePonto.DAL
{
    public class MomentoContext : DbContext
    {
        public MomentoContext(DbContextOptions options) : base(options) { }

        public MomentoContext() { }

        public virtual DbSet<Momento> Momentos { get; set; } = null!;
    }
}
