using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TemplateApi.Domain.Model;
using TemplateApi.Infrastructure.Data.Context;

namespace TemplateApi.Infrastructure.Data.Context
{
    public sealed class ProductConfiguration
        : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.Description);
            builder.Property(p => p.Price).HasPrecision(18, 2);
        }
    }
}