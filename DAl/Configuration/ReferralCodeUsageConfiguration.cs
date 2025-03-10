using DAl.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DAl.Configuration
{
    public class ReferralCodeUsageConfiguration : IEntityTypeConfiguration<ReferralCodeUsage>
    {
        public void Configure(EntityTypeBuilder<ReferralCodeUsage> builder)
        {

            builder.HasKey(ke => new { ke.OrderId, ke.UserId, ke.CodeId });

            // relation between
            builder
              .HasOne(rcu => rcu.ApplicationUser)
              .WithMany(u => u.ReferralCodeUsages).OnDelete(DeleteBehavior.Cascade)
              .HasForeignKey(rcu => rcu.UserId);

           builder
                .HasOne(rcu => rcu.CodesToMarketer)
                    .WithMany(c => c.ReferralCodeUsages)
                    .HasForeignKey(rcu => rcu.CodeId).OnDelete(DeleteBehavior.Restrict);

            builder
              .HasOne(rcu => rcu.Order)
              .WithOne(o => o.ReferralCodeUsage)
              .HasForeignKey<ReferralCodeUsage>(rcu => rcu.OrderId);
        }


    }
    }

