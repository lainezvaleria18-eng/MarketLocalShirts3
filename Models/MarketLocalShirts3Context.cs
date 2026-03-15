using Microsoft.EntityFrameworkCore;

namespace MarketLocalShirts3.Models
{
    public class MarketLocalShirts3Context : DbContext
    {
        public MarketLocalShirts3Context(DbContextOptions<MarketLocalShirts3Context> options) : base(options)
        {
        }

        public DbSet<Rol> Roles { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Marca> Marcas { get; set; }

        public DbSet<Producto> Productos { get; set; }

        public DbSet<Pedido> Pedidos { get; set; }

        public DbSet<DetallePedido> PedidosDetalles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Producto>().HasKey(p => p.Id);

            modelBuilder.Entity<Marca>().HasKey(m => m.Id);
            modelBuilder.Entity<Cliente>().HasKey(c => c.Id);
            modelBuilder.Entity<Pedido>().HasKey(p => p.Id);
            modelBuilder.Entity<Rol>().HasKey(r => r.Id);

        }
    }
}
