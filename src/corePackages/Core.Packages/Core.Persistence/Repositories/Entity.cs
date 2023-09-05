namespace Core.Persistence.Repositories
{
    public class Entity<TId>:IEntityTimestamps
    {
        //Tüm tablolarda Id sütunu olacağı için ve her bir tabloda farklı türden Id olacağından Generic Entity sınıfı oluşturuldu.
        public TId Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; } //ilk nesne girildiğinde Update tarihi girilmesi zorunda olmadığımdan nullable yapıldı.
        public DateTime? DeletedDate { get; set; } //ilk nesne girildiğinde Delete tarihi girilmesi zorunda olmadığımdan nullable yapıldı.

        public Entity()
        {
            Id = default;//Hiç bir şey verilmezse Id'nin defaultu verilsin mesela Id int olsun. Default değeri sıfırdır.
        }
        public Entity(TId id)
        {
            Id= id;
        }
    }
}
