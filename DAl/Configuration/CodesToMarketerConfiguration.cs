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
    public class CodesToMarketerConfiguration : IEntityTypeConfiguration<CodesToMarketer>
    {
        public void Configure(EntityTypeBuilder<CodesToMarketer> builder)
        {
          

            builder.HasKey(x => x.Id);

            builder.Property(x=>x.Code)
                .HasMaxLength(100)
                .IsRequired(); 


        }
    }
}
