﻿using System;
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
            modelBuilder.Entity<ProductType>()
                .HasMany(t => t.Products)
                .WithOptional(t => t.ProductType);

            modelBuilder.Entity<Product>()
                .HasMany(t => t.Purchases)
                .WithMany(t => t.Products);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Purchases)
                .WithOptional(p => p.Customer);

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public  DbSet<Customer> Customers { get; set; }
        //public DbSet<Performer> Performers { get; set; }
        //public DbSet<Album> Albums { get; set; }




    }
}
