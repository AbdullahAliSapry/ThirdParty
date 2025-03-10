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
    public class MarketerConfiguration : IEntityTypeConfiguration<Marketer>
    {
        public void Configure(EntityTypeBuilder<Marketer> builder)
        {


            builder.HasKey(m => m.Id);



            // ralation between Marketer and ApplicationUser
            builder.HasOne(m=>m.ApplicationUser)
                .WithOne(a=>a.Marketer)
                .HasForeignKey<Marketer>(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            // ralation between Marketer and CodesToMarketer
            builder.HasMany(m => m.CodesToMarketers)
                .WithOne(c => c.Marketer)
                .HasForeignKey(c => c.MarketerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
