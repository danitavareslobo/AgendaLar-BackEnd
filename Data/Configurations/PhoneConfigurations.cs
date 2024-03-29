﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Model = AgendaLarAPI.Models.People;

namespace AgendaLarAPI.Data.Configurations
{
    public class PhoneConfigurations : IEntityTypeConfiguration<Model.Phone>
    {
        public void Configure(EntityTypeBuilder<Model.Phone> builder)
        {
            builder.ToTable("Phones");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.UserId)
                .IsRequired()
                .HasColumnType("nvarchar(450)");

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

            builder.Property(p => p.Number)
                .IsRequired()
                .HasColumnType($"varchar({Model.Phone.NumberMaxLength})");

            builder.Property(p => p.Type)
                .IsRequired()
                .HasConversion<int>();

            builder.HasOne(p => p.Person)
                .WithMany(p => p.Phones)
                .HasForeignKey(p => p.PersonId);

            builder.HasOne(p => p.User)
                .WithMany(p => p.Phones)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
