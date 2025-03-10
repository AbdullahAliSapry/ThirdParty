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
    public class BillingToMarketrConfiguration : IEntityTypeConfiguration<BillingToMarketr>
    {
        public void Configure(EntityTypeBuilder<BillingToMarketr> builder)
        {


            // get ellement

            builder.HasKey(x => x.Id);

            builder.HasOne(e=>e.FileUploads)
                .WithOne(e=>e.BillingToMarketr)
                .HasForeignKey<BillingToMarketr>(e=>e.FileId);

        }
    }
}
