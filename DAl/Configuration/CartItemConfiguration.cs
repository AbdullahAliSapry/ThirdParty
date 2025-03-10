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
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {


            builder.HasKey(x => x.Id);

           builder.HasMany(e => e.AttributeItems)
                .WithOne(e=>e.CartItem)
                .HasForeignKey(e=>e.CartItemId).OnDelete(DeleteBehavior.Cascade);



            builder.HasOne(e => e.Category)
                .WithMany(e => e.CartItems)
                .HasForeignKey(e=>e.categoryId);

            builder.HasOne(e=>e.Order)
                .WithMany(e=>e.CartItems)
                .HasForeignKey(e=>e.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
