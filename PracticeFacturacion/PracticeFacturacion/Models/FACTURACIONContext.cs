using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PracticeFacturacion.Models
{
    public partial class FACTURACIONContext : DbContext
    {
        public FACTURACIONContext()
        {
        }

        public FACTURACIONContext(DbContextOptions<FACTURACIONContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cliente> Clientes { get; set; } = null!;
        public virtual DbSet<FacturacionDetail> FacturacionDetails { get; set; } = null!;
        public virtual DbSet<FacturacionHeader> FacturacionHeaders { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost; Database=FACTURACION; Trusted_Connection=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("CLIENTES");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Apellidos)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("APELLIDOS");

                entity.Property(e => e.Direccion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("DIRECCION");

                entity.Property(e => e.Documento)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DOCUMENTO");

                entity.Property(e => e.Fecha)
                    .HasColumnType("date")
                    .HasColumnName("FECHA")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE");
            });

            modelBuilder.Entity<FacturacionDetail>(entity =>
            {
                entity.ToTable("FACTURACION_DETAIL");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cantidad).HasColumnName("CANTIDAD");

                entity.Property(e => e.CostoUnitario).HasColumnName("COSTO_UNITARIO");

                entity.Property(e => e.DescripcionProducto)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION_PRODUCTO");

                entity.Property(e => e.IdHeader).HasColumnName("ID_HEADER");

                entity.Property(e => e.Itbis).HasColumnName("ITBIS");

                entity.Property(e => e.Total).HasColumnName("TOTAL");

                entity.HasOne(d => d.IdHeaderNavigation)
                    .WithMany(p => p.FacturacionDetails)
                    .HasForeignKey(d => d.IdHeader)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FACTURACION_DETAIL_FACTURACION_HEADER");
            });

            modelBuilder.Entity<FacturacionHeader>(entity =>
            {
                entity.ToTable("FACTURACION_HEADER");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClienteId).HasColumnName("CLIENTE_ID");

                entity.Property(e => e.FechaFactura)
                    .HasColumnType("date")
                    .HasColumnName("FECHA_FACTURA");

                entity.Property(e => e.Itbis).HasColumnName("ITBIS");

                entity.Property(e => e.TotalFacturado).HasColumnName("TOTAL_FACTURADO");

                entity.HasOne(d => d.Cliente)
                    .WithMany(p => p.FacturacionHeaders)
                    .HasForeignKey(d => d.ClienteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FACTURACION_HEADER_CLIENTES");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
