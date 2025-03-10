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
    public class PayMentManoulConfiguration : IEntityTypeConfiguration<PayMentManoul>
    {
        public void Configure(EntityTypeBuilder<PayMentManoul> builder)
        {

            builder.HasKey(x => x.Id);


            builder.HasOne(e => e.User).WithMany(e => e.PayMentManoul)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Order)
                .WithOne(e => e.PayMentManoul)
                .HasForeignKey<PayMentManoul>(e => e.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.FileUploads)
            .WithOne(e => e.PayMentManoul)
            .HasForeignKey<PayMentManoul>(e => e.FileId)
            .OnDelete(DeleteBehavior.Cascade);


            builder.HasOne(e => e.Account)
            .WithMany(e => e.PayMentManouls)
            .HasForeignKey(e => e.AccountId)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
