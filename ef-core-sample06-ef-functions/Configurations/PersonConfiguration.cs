using MasteringEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasteringEFCore.Configurations
{
    // // TPH CONFIGURATION
    // public class PersonConfiguration : IEntityTypeConfiguration<Person>
    // {
    //     public void Configure(EntityTypeBuilder<Person> builder)
    //     {
    //         builder.ToTable("People")
    //                .HasDiscriminator<int>("PersonType")
    //                .HasValue<Person>(3)
    //                .HasValue<Teacher>(6)
    //                .HasValue<Student>(99);
    //     }
    // }

    // TPT CONFIGURATION
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable("People");
        }
    }

    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.ToTable("Teachers");
        }
    }

    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students");
        }
    }
}
