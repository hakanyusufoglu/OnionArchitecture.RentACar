using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations
{
    public class CarConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.ToTable("Cars").HasKey(b => b.Id);//Car veri tabanında Cars Tablo ismi olsun diyoruz.
            builder.Property(b => b.Id).HasColumnName("Id").IsRequired(); //Veri tabanında Id alanına karşılık gelir ve required'tir.
            builder.Property(b => b.ModelId).HasColumnName("ModelId").IsRequired();
            builder.Property(b => b.Kilometer).HasColumnName("Kilometer").IsRequired();
            builder.Property(b => b.CarState).HasColumnName("State").IsRequired();
            builder.Property(b => b.ModelYear).HasColumnName("ModelYear").IsRequired();

            //Car'e global filtreleme eklenmesini sağlıyor. (GlobalQuery)
            //DeletedDate alanında veri yoksa 
            builder.HasQueryFilter(b => !b.DeletedDate.HasValue);

            //Bir Car'ın birden bir modeli olabilir.
            builder.HasOne(b => b.Model);
        }
    }
}
