using Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class EStoreDBContext : DbContext
    {
        public EStoreDBContext(DbContextOptions<EStoreDBContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Consultant>()
                    .HasOne(e => e.Recomendator)
                    .WithMany()
                    .OnDelete(DeleteBehavior.NoAction);
        }

        public DbSet<Consultant> Consultants { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }

    }
}
