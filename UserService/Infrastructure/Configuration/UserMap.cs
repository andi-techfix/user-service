using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class UserMap : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> b)
    {
        b.ToTable("Users");
        
        b.HasKey(x => x.Id);
        b.Property(x => x.Id)
            .HasColumnName("Id")                     
            .ValueGeneratedOnAdd()                  
            .UseIdentityByDefaultColumn();          

        b.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        b.Property(x => x.Email)
            .HasConversion(
                email => email.Value,
                value => Email.Create(value).Value
            )
            .HasMaxLength(200)
            .IsRequired();

        b.HasOne(x => x.Subscription)
            .WithMany()
            .HasForeignKey(x => x.SubscriptionId)
            .IsRequired();
    }
}