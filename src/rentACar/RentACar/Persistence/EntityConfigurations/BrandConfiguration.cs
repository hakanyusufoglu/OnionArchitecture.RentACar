using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations
{
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.ToTable("Brands").HasKey(b => b.Id);//Brand veri tabanında Brands Tablo ismi olsun diyoruz.
            builder.Property(b => b.Id).HasColumnName("Id").IsRequired(); //Veri tabanında Id alanına karşılık gelir ve required'tir.
            builder.Property(b => b.Name).HasColumnName("Name").IsRequired();
            builder.Property(b => b.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            builder.Property(b => b.UpdatedDate).HasColumnName("UpdatedDate");
            builder.Property(b => b.DeletedDate).HasColumnName("DeletedDate");
            //Brand'e global filtreleme eklenmesini sağlıyor. (GlobalQuery)

            //DeletedDate alanında veri yoksa 
            builder.HasQueryFilter(b => !b.DeletedDate.HasValue);
            
            //Veri tabanında marka isimleri tekrar etmesini engeller
            builder.HasIndex(indexExpression:b=>b.Name,name:"UK_Brands_Name").IsUnique();
            //Bir brand'ın birden fazla modeli olabilir.
            builder.HasMany(b => b.Models);
        }
    }
}
