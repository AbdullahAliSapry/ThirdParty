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
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasKey(x => x.Id);


            builder.HasOne(e => e.User)
                .WithOne(e => e.Cart)
                .HasForeignKey<Cart>(e=>e.UserId);




            builder.HasMany(e => e.CartItems)
                .WithOne(e => e.Cart).HasForeignKey(e=>e.CartId)
                .OnDelete(DeleteBehavior.Cascade);
                        


        }
    }
}
