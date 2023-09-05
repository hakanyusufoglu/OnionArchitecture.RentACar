namespace Core.Persistence.Dynamic
{
    //Sort ve Filter yardımcı sınıflarımı kullanıcağım sınıftır.
    public class DynamicQuery
    {
        //ToDo: Sorts olarak değiştir.
        public IEnumerable<Sort>? Sort { get; set; }
        public Filter? Filter { get; set; }  //Zaten filtre içinde IEnumurable olduğu için yani iç içe zincir şeklinde filtrelerim olacağı için burada tekil belirledik
        public DynamicQuery()
        {
            
        }
        public DynamicQuery(IEnumerable<Sort>? sort, Filter? filter)
        {
            Sort = sort;
            Filter = filter;
        }
    }
   
}
