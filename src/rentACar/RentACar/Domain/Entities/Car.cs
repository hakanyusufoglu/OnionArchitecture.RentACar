using Core.Persistence.Repositories;
using Domain.Enums;

namespace Domain.Entities
{
    public class  Car:Entity<Guid>
    {
        public Guid ModelId { get; set; }
        public int Kilometer { get; set; }//Araç kaç kilometrededir.
        public short ModelYear { get; set; }
        public string Plate { get; set; }
        public short MinFindexScore { get; set; } //Güvenirlilik değeri
        public CarState CarState { get; set; }

        public virtual Model? Model { get; set; } //Bir arabanın tek modeli olur
        public Car()
        {
           
        }

        public Car(Guid id,Guid modelId, int kilometer, short modelYear, string plate, short minFindexScore, CarState carState)
        {
            Id=id;
            ModelId = modelId;
            Kilometer = kilometer;
            ModelYear = modelYear;
            Plate = plate;
            MinFindexScore = minFindexScore;
            CarState = carState;
        }
    }
}
