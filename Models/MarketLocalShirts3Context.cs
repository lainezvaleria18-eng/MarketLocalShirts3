using Microsoft.EntityFrameworkCore;

namespace MarketLocalShirts3.Models
{
    public partial class MarketLocalShirts3Context : DbContext
    {
        public MarketLocalShirts3Context()
        {
        }

        public MarketLocalShirts3Context(DbContextOptions<MarketLocalShirts3Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<Rol> Rols { get; set; }
        public virtual DbSet<Producto> Productos { get; set; }
        public virtual DbSet<Marca> Marcas { get; set; }
        public virtual DbSet<Pedido> Pedidos { get; set; }
        public virtual DbSet<DetallePedido> DetallePedidos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rol>(entity =>
            {
                entity.HasKey(e => e.IdRol);
                entity.ToTable("Rol");

                entity.Property(e => e.NombreRol)
                    .HasMaxLength(50)
                    .IsRequired();
            });

            modelBuilder.Entity<Marca>(entity =>
            {
                entity.HasKey(e => e.IdMarca);
                entity.ToTable("Marca");

                entity.Property(e => e.NombreMarca)
                    .HasMaxLength(100)
                    .IsRequired();
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario);
                entity.ToTable("Usuario");

                entity.Property(e => e.Nombre).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Correo).HasMaxLength(150).IsRequired();
                entity.Property(e => e.Contrasena).HasMaxLength(200).IsRequired();

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.IdRol);
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.IdProducto);
                entity.ToTable("Producto");

                entity.Property(e => e.NombreProducto)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(e => e.Precio)
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.Imagen)
                    .HasMaxLength(255);

                entity.HasOne(d => d.IdMarcaNavigation)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.IdMarca);
            });

            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.HasKey(e => e.IdPedido);
                entity.ToTable("Pedido");

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Pedidos)
                    .HasForeignKey(d => d.IdUsuario);
            });

            modelBuilder.Entity<DetallePedido>(entity =>
            {
                entity.HasKey(e => e.IdDetalle);
                entity.ToTable("DetallePedido");

                entity.Property(e => e.PrecioUnitario)
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.Subtotal)
                    .HasColumnType("decimal(10,2)");

                entity.HasOne(d => d.IdPedidoNavigation)
                    .WithMany(p => p.DetallePedidos)
                    .HasForeignKey(d => d.IdPedido);

                entity.HasOne(d => d.IdProductoNavigation)
                    .WithMany(p => p.DetallePedidos)
                    .HasForeignKey(d => d.IdProducto);
            });
        }
    }
}