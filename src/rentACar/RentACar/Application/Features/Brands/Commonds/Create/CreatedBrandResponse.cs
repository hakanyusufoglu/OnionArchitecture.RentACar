namespace Application.Features.Brands.Commonds.Create
{
    public class CreatedBrandResponse
    {
        //requeste karşılık response oluyor ve bu response olarak ne döndürmek istiyorum?
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; } //Create'da sadece create tarihi olabilir.
    }
}
