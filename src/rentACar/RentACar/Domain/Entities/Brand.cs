using Core.Persistence.Repositories;

namespace Domain.Entities
{
    public class Brand : Entity<Guid> //tüm alanlarda ortak olacak şeyler corePackages klasörü içerisinde toparlanır
    {
        public string Name { get; set; }

        public virtual ICollection<Model> Models { get; set; }
        //Constructos kullanım kolaylığını sağlamak için gerçekleştirilmiştir.
        public Brand()
        {
            Models = new HashSet<Model>();
        }
        public Brand(Guid id, string name):this() //parametresiz constructorda çalışsın
        {
            Id = id;
            Name= name;
        }
    }
}
