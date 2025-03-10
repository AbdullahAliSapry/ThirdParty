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
    class CommissionSchemeConfiguration : IEntityTypeConfiguration<CommissionScheme>
    {
        public void Configure(EntityTypeBuilder<CommissionScheme> builder)
        {


            builder.HasKey(c => c.Id);


            builder.Property(c => c.LowerLimit)
                .IsRequired();

            builder.Property(c => c.UpperLimit)
                .IsRequired();



        }
    }
}
