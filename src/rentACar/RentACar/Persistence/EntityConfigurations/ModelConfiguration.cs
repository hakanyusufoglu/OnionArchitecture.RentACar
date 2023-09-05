using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations
{
    public class ModelConfiguration : IEntityTypeConfiguration<Model>
    {
        public void Configure(EntityTypeBuilder<Model> builder)
        {
            builder.ToTable("Models").HasKey(b => b.Id);//Model veri tabanında Models Tablo ismi olsun diyoruz.
           
            builder.Property(b => b.Id).HasColumnName("Id").IsRequired(); //Veri tabanında Id alanına karşılık gelir ve required'tir.
            builder.Property(b => b.Name).HasColumnName("Name").IsRequired();
            builder.Property(b => b.BrandId).HasColumnName("BrandId").IsRequired();
            builder.Property(b => b.FuelId).HasColumnName("FuelId").IsRequired();
            builder.Property(b => b.TransmissionId).HasColumnName("TransmissionId").IsRequired();
            builder.Property(b => b.DailyPrice).HasColumnName("DailyPrice").IsRequired();
            builder.Property(b => b.ImageUrl).HasColumnName("ImageUrl").IsRequired();


            builder.Property(b => b.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            builder.Property(b => b.UpdatedDate).HasColumnName("UpdatedDate");
            builder.Property(b => b.DeletedDate).HasColumnName("DeletedDate");
            
            //Model'e global filtreleme eklenmesini sağlıyor. (GlobalQuery)
            //DeletedDate alanında veri yoksa 
            builder.HasQueryFilter(b => !b.DeletedDate.HasValue);

            //Bir modelin bir markası olur
            builder.HasOne(b => b.Brand);
            builder.HasOne(b => b.Fuel);
            builder.HasOne(b => b.Transmission);

            //Bir modelden birden fazla araba olabilir
            builder.HasMany(b => b.Cars);
        }
    }
}
