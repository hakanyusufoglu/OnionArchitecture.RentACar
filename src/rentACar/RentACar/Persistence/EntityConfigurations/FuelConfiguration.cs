using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations
{
    public class FuelConfiguration : IEntityTypeConfiguration<Fuel>
    {
        public void Configure(EntityTypeBuilder<Fuel> builder)
        {
            builder.ToTable("Fuels").HasKey(b => b.Id);//Fuel veri tabanında Fuels Tablo ismi olsun diyoruz.
            builder.Property(b => b.Id).HasColumnName("Id").IsRequired(); //Veri tabanında Id alanına karşılık gelir ve required'tir.
            builder.Property(b => b.Name).HasColumnName("Name").IsRequired();
            builder.Property(b => b.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            builder.Property(b => b.UpdatedDate).HasColumnName("UpdatedDate");
            builder.Property(b => b.DeletedDate).HasColumnName("DeletedDate");
            //Fuels'e global filtreleme eklenmesini sağlıyor. (GlobalQuery)

            //DeletedDate alanında veri yoksa 
            builder.HasQueryFilter(b => !b.DeletedDate.HasValue);

            //Veri tabanında marka isimleri tekrar etmesini engeller
            builder.HasIndex(indexExpression: b => b.Name, name: "UK_Fuels_Name").IsUnique();
           
            //Bir fuel'ın birden fazla modeli olabilir.
            builder.HasMany(b => b.Models);
        }
    }
}
