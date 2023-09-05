namespace Application.Features.Brands.Queries.GetList
{
    //Kullanıcıya hangi bilgileri vermek istiyorsam burada tanımlanır.
    public class GetListBrandListItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
