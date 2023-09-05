using Core.Persistence.Repositories;

namespace Domain.Entities
{
    //Araç Modeli
    public class Model : Entity<Guid>
    {
        public Guid BrandId { get; set; }
        public Guid FuelId { get; set; }
        public Guid TransmissionId { get; set; }
        public string Name { get; set; }
        public decimal DailyPrice { get; set; }
        public string ImageUrl { get; set; }

        public virtual Brand? Brand { get; set; }
        public virtual Fuel? Fuel { get; set; }
        public virtual Transmission? Transmission { get; set; }
        public virtual ICollection<Car> Cars { get; set; } //Bir modelin birden fazla arabası olur.
        public Model()
        {
            Cars = new HashSet<Car>();
        }
        //Yardımcı constructor
        public Model(Guid id, Guid brandId, Guid fuelId, Guid transmissionId, string name, decimal dailyPrice, string imageUrl ):this()//parametresiz constructor da çalışır
        {
            Id= id;
            BrandId= brandId;
            FuelId= fuelId;
            TransmissionId= transmissionId;
            Name= name;
            DailyPrice= dailyPrice;
            ImageUrl= imageUrl;
        }
    }
}
