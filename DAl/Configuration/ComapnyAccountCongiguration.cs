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
    public class ComapnyAccountCongiguration : IEntityTypeConfiguration<ComapnyAccount>
    {
        public void Configure(EntityTypeBuilder<ComapnyAccount> builder)
        {
            builder.HasKey(x => x.Id);



            builder.HasOne(e => e.FileUploads).WithOne(e => e.ComapnyAccount)
                .HasForeignKey<ComapnyAccount>(e=>e.FileId);


            builder.HasOne(e=>e.User).WithOne(e => e.ComapnyAccount)
                .HasForeignKey<ComapnyAccount>(e=>e.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
