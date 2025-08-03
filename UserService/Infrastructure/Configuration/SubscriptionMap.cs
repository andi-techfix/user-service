using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class SubscriptionMap : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> b)
    {
        b.ToTable("Subscriptions", t=> 
        {
            t.HasCheckConstraint(
                name: "CK_Subscriptions_EndDate_After_StartDate",
                sql: "\"EndDate\" > \"StartDate\""
            );
            
            t.HasCheckConstraint("CK_Subscriptions_EndDate_In_Future",
                "\"EndDate\" > NOW()");
        });
        
        b.HasKey(x => x.Id);
        b.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityByDefaultColumn();

        b.Property(x => x.Type)
            .HasConversion<int>()
            .IsRequired();

        b.Property(x => x.StartDate)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        b.Property(x => x.EndDate)
            .IsRequired();
    }
}