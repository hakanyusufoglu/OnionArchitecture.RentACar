using Core.Persistence.Repositories;

namespace Domain.Entities
{
    //Yakıt tipi
    public class Fuel : Entity<Guid>
    {
        public string Name { get; set; }

        public virtual ICollection<Model> Models { get; set; } // Bir yakıta ait biden fazla model vardır.
        public Fuel()
        {
            Models = new HashSet<Model>();
        }
        public Fuel(Guid id, string name) : this()
        {
            Id = id;
            Name = name;
        }

    }
}
