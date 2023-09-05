using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations
{
    public class TransmissionConfiguration : IEntityTypeConfiguration<Transmission>
    {
        public void Configure(EntityTypeBuilder<Transmission> builder)
        {
            builder.ToTable("Transmissions").HasKey(b => b.Id);//Transmission veri tabanında Transmissions Tablo ismi olsun diyoruz.
            builder.Property(b => b.Id).HasColumnName("Id").IsRequired(); //Veri tabanında Id alanına karşılık gelir ve required'tir.
            builder.Property(b => b.Name).HasColumnName("Name").IsRequired();
            builder.Property(b => b.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            builder.Property(b => b.UpdatedDate).HasColumnName("UpdatedDate");
            builder.Property(b => b.DeletedDate).HasColumnName("DeletedDate");
            //Transmission'e global filtreleme eklenmesini sağlıyor. (GlobalQuery)

            //DeletedDate alanında veri yoksa 
            builder.HasQueryFilter(b => !b.DeletedDate.HasValue);

            //Veri tabanında Transmission namelerini tekrar etmesini engeller
            builder.HasIndex(indexExpression: b => b.Name, name: "UK_Transmissions_Name").IsUnique();
           
            //Bir Transmission'ın birden fazla modeli olabilir.
            builder.HasMany(b => b.Models);
        }
    }
}
