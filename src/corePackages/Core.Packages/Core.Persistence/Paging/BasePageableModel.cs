namespace Core.Persistence.Paging
{
    //İlerleyen ihtiyaçlara genişletilebileceği için abstarct yapıldı
    public abstract class BasePageableModel
    {
        //Sayfamızda kaç veri var?
        public int Size { get; set; }
        //Kaçıncı sayfadayız?
        public int Index { get; set; }
        public int Count { get; set; }
        public int Pages { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }
    }
}
