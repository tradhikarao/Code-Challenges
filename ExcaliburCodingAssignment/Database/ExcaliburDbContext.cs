using ExcaliburCodingAssignment.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExcaliburCodingAssignment.Database
{
    public class ExcaliburDbContext: DbContext
    {
        public DbSet<OrderDate> OrderDate { get; set; }
        
        public DbSet<OrderDetail> OrderDetail { get; set; }

        public DbSet<OrderCombined> OrderCombined { get; set; }

        public ExcaliburDbContext(DbContextOptions<ExcaliburDbContext>options): base(options)
        {

        }

        //Seed the OrderDate & OrderDetails table using the PK of the OrderDate as the FK into the OrderDetails table
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDate>().HasData(
                new OrderDate
                {
                    OrderID = 20,
                    OrderedDate = DateTime.Parse("2020-08-01")
                },
                 new OrderDate
                 {
                     OrderID = 21,
                     OrderedDate = DateTime.Parse("2020-08-15")
                 },
                  new OrderDate
                  {
                      OrderID = 22,
                      OrderedDate = DateTime.Parse("2020-09-01")
                  },
                  new OrderDate
                  {
                      OrderID = 23,
                      OrderedDate = DateTime.Parse("2020-09-15")
                  }
                );

            modelBuilder.Entity<OrderDetail>().HasData(
               new OrderDetail
               {
                   Id = 20,
                   OrderId = 20,
                   OrderAmount = 2000,
                   OrderDesc = "Order 2000"
               },
                new OrderDetail
                {
                    Id = 21,
                    OrderId = 21,
                    OrderAmount = 2100,
                    OrderDesc = "Order 2100"
                },
                 new OrderDetail
                 {
                     Id = 22,
                     OrderId = 22,
                     OrderAmount = 2200,
                     OrderDesc = "Order 2200"
                 },
                 new OrderDetail
                 {
                     Id = 23,
                     OrderId = 23,
                     OrderAmount = 2300,
                     OrderDesc = "Order 2300"
                 }
               );
        }
    }
}
