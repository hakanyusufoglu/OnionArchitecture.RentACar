using Core.Persistence.Repositories;

namespace Domain.Entities
{
    //Vites türü
    public class Transmission : Entity<Guid>
    {
        public string Name { get; set; }

        public virtual ICollection<Model> Models { get; set; } // Bir yakıta ait biden fazla model vardır.
        public Transmission()
        {
            Models = new HashSet<Model>();
        }
        public Transmission(Guid id, string name) : this()
        {
            Id = id;
            Name = name;
        }
    }
}
