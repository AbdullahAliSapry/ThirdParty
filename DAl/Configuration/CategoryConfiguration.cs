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
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(e=>e.NameAr)
                .IsRequired(false);

            builder.HasMany(e=>e.SubCategories)
                .WithOne(e=>e.Parent)
                .HasForeignKey(e=>e.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
