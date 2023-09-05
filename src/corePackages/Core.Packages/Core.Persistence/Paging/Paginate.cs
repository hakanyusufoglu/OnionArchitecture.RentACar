namespace Core.Persistence.Paging
{
    //Sayfalama işlemleri için gerçekleştirilen sınıftır.
    public class Paginate<T>
    {
        public Paginate()
        {
            Items=Array.Empty<T>();
        }
        //Sayfamızda kaç veri var?
        public int Size { get; set; }
        
        //Kaçıncı sayfadayız?
        public int Index { get; set; }
        public int Count { get; set; }
        public int Pages { get; set; }
        public IList<T> Items { get; set; }
        public bool HasPrevious => Index > 0;// yani 1. sayfada bir önceki sayfa var

        public bool HasNext => Index+1 < Pages;
    }
}
