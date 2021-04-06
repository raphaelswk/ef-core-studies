using MasteringEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasteringEFCore.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            // OWNED TYPES
            builder.OwnsOne(x => x.Address, address =>
            {
                address.Property(p => p.Neighbour).HasColumnName("Neighbour");

                address.ToTable("Addresses");
            });
        }
    }
}
