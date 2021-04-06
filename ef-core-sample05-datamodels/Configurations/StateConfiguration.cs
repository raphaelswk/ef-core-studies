using MasteringEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasteringEFCore.Configurations
{
    public class StateConfiguration : IEntityTypeConfiguration<State>
    {
        public void Configure(EntityTypeBuilder<State> builder)
        {
            // CONFIGURING RELATIONSHIP ONE TO ONE
            builder.HasOne(p => p.Governor)
                   .WithOne(p => p.State)
                   .HasForeignKey<Governor>(p => p.StateReference);

            // CONFIGURATION TO MAKE IT LOAD Governor AUTOMATICALLY (NO NEED TO ADD INCLUDE)
            builder.Navigation(p => p.Governor).AutoInclude();

            // CONFIGURING RELATIONSHIP ONE TO MANY
            builder.HasMany(p => p.Cities)
                   .WithOne(p => p.State)
                   .IsRequired(false) // DOES NOT REQUIRE A STATE TO CREATE A CITY (NOT RECOMMENDED)
                   .OnDelete(DeleteBehavior.Restrict); // ON DELETE BEHAVIOR CAN BE CHANGED
        }
    }    
}
