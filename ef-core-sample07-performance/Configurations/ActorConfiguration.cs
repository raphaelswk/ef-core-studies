using System;
using System.Collections.Generic;
using MasteringEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasteringEFCore.Configurations
{
    public class ActorConfiguration : IEntityTypeConfiguration<Actor>
    {
        public void Configure(EntityTypeBuilder<Actor> builder)
        {
            // CONFIGURING RELATIONSHIP MANY TO MANY
            builder.HasMany(p => p.Movies)
                   .WithMany(p => p.Actors)
                   .UsingEntity<Dictionary<string, object>>(
                       "MoviesActors",
                       p => p.HasOne<Movie>().WithMany().HasForeignKey("MovieId"),
                       p => p.HasOne<Actor>().WithMany().HasForeignKey("ActorId"),
                       p => 
                       {
                           p.Property<DateTime>("CreatedOn").HasDefaultValueSql("GETDATE()");
                       }
                   );

            // builder.HasMany(p => p.Movies)
            //        .WithMany(p => p.Actors)
            //        .UsingEntity(p => p.ToTable("ActorsMovies")); // CUSTOM TABLE NAME
        }
    }    
}
