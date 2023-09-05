namespace Core.Persistence.Dynamic
{
    //A-Z, Z-A ya sıralama için kullanılan bir sınıftır (Yardımcı sınıf)
    public class Sort
    {
        public string Field { get; set; }
        public string Dir { get; set; } // Direction: yani A-Z, Z-A ya sıralama için
        public Sort()
        {
            Field = string.Empty;
            Dir = string.Empty;
        }
        public Sort(string field, string dir) 
        {
            Field = field;
            Dir = dir;
        }
    }
}
