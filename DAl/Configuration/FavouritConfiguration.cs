using DAl.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAl.Configuration
{
    public class FavouritConfiguration : IEntityTypeConfiguration<Favorite>
    {
        public void Configure(EntityTypeBuilder<Favorite> builder)
        {
           
            


            builder.HasKey(x => x.Id);



            builder.HasMany(e=>e.FavoriteItems)
                .WithOne(e => e.Favorite)
                .HasForeignKey(e => e.FavoriteId)
                .OnDelete(DeleteBehavior.Cascade);



            builder.HasOne(e => e.User).WithOne(e => e.Favorite)
                .HasForeignKey<Favorite>(e => e.UserId).OnDelete(DeleteBehavior.Cascade);




            builder.HasMany(e=>e.FavoriteSallers)
                .WithOne(e=>e.Favorite)
                .HasForeignKey(e=>e.FavoriteId)
                .OnDelete(DeleteBehavior.Cascade);





        }
    }
}
