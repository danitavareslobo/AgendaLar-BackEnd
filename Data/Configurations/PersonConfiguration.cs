using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Model = AgendaLarAPI.Models.Person;

namespace AgendaLarAPI.Data.Configurations
{
    public class PersonConfigurations : IEntityTypeConfiguration<Model.Person>
    {
        public void Configure(EntityTypeBuilder<Model.Person> builder)
        {
            builder.ToTable("People");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.CreatedAt)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(p => p.UpdatedAt)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(p => p.IsActive)
                .IsRequired()
                .HasColumnType("bit");

            builder.Property(p => p.IsDeleted)
                .IsRequired()
                .HasColumnType("bit");

            builder.Property(p => p.Name)
                .IsRequired()
                .HasColumnType($"varchar({Model.Person.NameMaxLength})");

            builder.Property(p => p.Email)
                .IsRequired()
                .HasColumnType($"varchar({Model.Person.EmailMaxLength})");

            builder.Property(p => p.SocialNumber)
                .IsRequired()
                .HasColumnType($"varchar({Model.Person.SocialNumberMaxLength})");

            builder.Property(p => p.BirthDate)
                .IsRequired()
                .HasColumnType("datetime");

            builder.HasMany(p => p.Phones)
                .WithOne(p => p.Person)
                .HasForeignKey(p => p.PersonId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
