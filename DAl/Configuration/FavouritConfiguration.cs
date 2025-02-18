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

            builder.HasMany(e => e.Users)
                .WithMany(e => e.Favorites)
                .UsingEntity<FavouritUser>(
                     j =>
                     {
                                     j.HasOne(x => x.User)
                                     .WithMany(x => x.FavouritUsers)
                                     .HasForeignKey(e => e.UserId);

                                     j.HasOne(x => x.Favorite)
                                         .WithMany(x => x.FavouritUsers)
                                         .HasForeignKey(e => e.FavoriteId);
                                     j.HasKey(x => new { x.UserId, x.FavoriteId });

                     }

                );





        }
    }
}
