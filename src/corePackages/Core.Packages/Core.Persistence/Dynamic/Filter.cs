namespace Core.Persistence.Dynamic
{
    //Filtreleme işlemleri için (Yardımcı sınıf)
    public class Filter
    {
        //Filtreleme hangi alanlar için geçerli olacaktır. Örneğin aracın Yakıt filtresi vs. (Örneğin hepsiburada giyim filtresi vs gibi.
        public string Field { get; set; }
        public string? Value { get; set; }
        public string? Operator { get; set; } // => <= vs 
        public string? Logic { get; set; } // İçerisinde şu olsun gibi mantıksal ifade ve-veya
        public IEnumerable<Filter>? Filters { get; set; } //Bir filtreye başka filtreler ekleyebilirim.
        public Filter()
        {
            Field = string.Empty;
            Operator = string.Empty;
        }
        public Filter(string field, string @operator) //zaten operator diye bir keyword olacağı için ben kendi keywordümü kullanmak istiyorum diyorum (@ işareti ile)
        {
            Field = field;
            Operator = @operator;
        }
    }

}
