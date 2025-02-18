using DAl.Configuration;
using DAl.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAl
{
    public class ApplicationDbContext
        : IdentityDbContext<ApplicationUser>
    {

        public DbSet<Category> Categories { get; set; }
        //public DbSet<Marketer> Marketers { get; set; }
        //public DbSet<CodesToMarketer> CodesToMarketers { get; set; }
        //public DbSet<Proplem> Proplems { get; set; }    


        public DbSet<Favorite> Favorites { get; set; }



        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            new CategoryConfiguration()
                .Configure(builder.Entity<Category>());

            //new CodesToMarketerConfiguration()
            //    .Configure(builder.Entity<CodesToMarketer>());


            new FavouritConfiguration()
                .Configure(builder.Entity<Favorite>());

            builder.Entity<FavouritUser>()
                .ToTable("FavouritUsers");

        }

    }
}
