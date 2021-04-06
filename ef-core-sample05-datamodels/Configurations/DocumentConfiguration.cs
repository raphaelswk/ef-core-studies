using MasteringEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasteringEFCore.Configurations
{
    public class DocumentConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.Property("_pps")
                   .HasColumnName("PPS")
                   .HasMaxLength(11);

            // builder.Property(p => p.PPS).HasField("_pps");
        }
    }
}
