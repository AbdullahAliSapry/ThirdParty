using DAl.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Rest.Accounts.V1;

namespace DAl.Configuration
{
    public class MarketerAccountConfiguration : IEntityTypeConfiguration<MarketerAccount>
    {
        public void Configure(EntityTypeBuilder<MarketerAccount> builder)
        {
           

            builder.HasKey(x => x.Id);

            builder.HasOne(e=>e.Marketer)
                .WithMany(e=>e.MarketerAccounts).OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(e=>e.MarketerId);


            builder.HasMany(e=>e.BillingToMarketrs)
                .WithOne(e=>e.MarketerAccount).
                HasForeignKey(e=>e.MarkterAccountId).OnDelete(DeleteBehavior.Cascade);


        }
    }
}
