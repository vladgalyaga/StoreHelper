using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreHelper.Dal.Core.Interfaces;
using StoreHelperDAL.Models;

namespace StoreHelperDAL
{
   public class StoreHelperContext : DbContext
    {
        public StoreHelperContext() : base("StoreHelper")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Chat>()
            //    .HasMany(t => t.Users)
            //    .WithMany(t => t.Chats);

            //modelBuilder.Entity<User>()
            //    .HasMany(t => t.Events)
            //    .WithMany(t => t.Participants);

            //modelBuilder.Entity<User>()
            //    .HasMany(t => t.CreatedEvents)
            //    .WithOptional(t => t.Creater);
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        //public DbSet<Performer> Performers { get; set; }
        //public DbSet<Album> Albums { get; set; }




    }
}
