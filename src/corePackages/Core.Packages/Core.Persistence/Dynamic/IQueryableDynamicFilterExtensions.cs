using System.Linq.Dynamic.Core; // Linq.Dynamic.Core kütüphanesini kullanabilmek için gerekli
using System.Text;

namespace Core.Persistence.Dynamic
{
    public static class IQueryableDynamicFilterExtensions
    {
        private static readonly string[] _orders = { "asc", "desc" }; // Geçerli sıralama türleri
        private static readonly string[] _logics = { "and", "or" }; // Geçerli mantıksal operatörler

        private static readonly IDictionary<string, string> _operators = new Dictionary<string, string>
        {
            // Filtreleme operatörleri ve karşılık gelen işaretleri
            { "eq", "=" },
            { "neq", "!=" },
            { "lt", "<" },
            { "lte", "<=" },
            { "gt", ">" },
            { "gte", ">=" },
            { "isnull", "== null" },
            { "isnotnull", "!= null" },
            { "startswith", "StartsWith" },
            { "endswith", "EndsWith" },
            { "contains", "Contains" },
            { "doesnotcontain", "Contains" }
        };

        public static IQueryable<T> ToDynamic<T>(this IQueryable<T> query, DynamicQuery dynamicQuery)
        {
            if (dynamicQuery.Filter is not null)
                query = Filter(query, dynamicQuery.Filter); // Filtreleme işlemi
            if (dynamicQuery.Sort is not null && dynamicQuery.Sort.Any())
                query = Sort(query, dynamicQuery.Sort); // Sıralama işlemi
            return query;
        }

        private static IQueryable<T> Filter<T>(IQueryable<T> queryable, Filter filter)
        {
            IList<Filter> filters = GetAllFilters(filter); // Tüm filtreleri al
            string?[] values = filters.Select(f => f.Value).ToArray(); // Filtre değerlerini dizi olarak al
            string where = Transform(filter, filters); // Filtreleri dönüştür ve sorgu ifadesini oluştur
            if (!string.IsNullOrEmpty(where) && values != null)
                queryable = queryable.Where(where, values); // Sorguyu filtrelerle uygula

            return queryable;
        }

        private static IQueryable<T> Sort<T>(IQueryable<T> queryable, IEnumerable<Sort> sort)
        {
            foreach (Sort item in sort)
            {
                if (string.IsNullOrEmpty(item.Field))
                    throw new ArgumentException("Invalid Field"); // Geçersiz alan kontrolü
                if (string.IsNullOrEmpty(item.Dir) || !_orders.Contains(item.Dir))
                    throw new ArgumentException("Invalid Order Type"); // Geçersiz sıralama türü kontrolü
            }

            if (sort.Any())
            {
                string ordering = string.Join(separator: ",", values: sort.Select(s => $"{s.Field} {s.Dir}"));
                return queryable.OrderBy(ordering); // Sorguyu belirtilen sıralama ile sırala
            }

            return queryable;
        }

        public static IList<Filter> GetAllFilters(Filter filter)
        {
            List<Filter> filters = new(); // Filtreleri depolamak için bir liste oluştur
            GetFilters(filter, filters); // Tüm filtreleri al
            return filters;
        }

        private static void GetFilters(Filter filter, IList<Filter> filters)
        {
            filters.Add(filter); // Filtreyi listeye ekle
            if (filter.Filters is not null && filter.Filters.Any())
                foreach (Filter item in filter.Filters)
                    GetFilters(item, filters); // Alt filtreleri listeye ekle
        }

        public static string Transform(Filter filter, IList<Filter> filters)
        {
            if (string.IsNullOrEmpty(filter.Field))
                throw new ArgumentException("Invalid Field"); // Geçersiz alan kontrolü
            if (string.IsNullOrEmpty(filter.Operator) || !_operators.ContainsKey(filter.Operator))
                throw new ArgumentException("Invalid Operator"); // Geçersiz operatör kontrolü

            int index = filters.IndexOf(filter);
            string comparison = _operators[filter.Operator]; // Karşılaştırma operatörünü al
            StringBuilder where = new(); // Sorgu ifadesi oluşturmak için StringBuilder

            if (!string.IsNullOrEmpty(filter.Value))
            {
                if (filter.Operator == "doesnotcontain")
                    where.Append($"(!np({filter.Field}).{comparison}(@{index.ToString()}))");
                else if (comparison is "StartsWith" or "EndsWith" or "Contains")
                    where.Append($"(np({filter.Field}).{comparison}(@{index.ToString()}))");
                else
                    where.Append($"np({filter.Field}) {comparison} @{index.ToString()}");
            }
            else if (filter.Operator is "isnull" or "isnotnull")
            {
                where.Append($"np({filter.Field}) {comparison}");
            }

            if (filter.Logic is not null && filter.Filters is not null && filter.Filters.Any())
            {
                if (!_logics.Contains(filter.Logic))
                    throw new ArgumentException("Invalid Logic"); // Geçersiz mantıksal operatör kontrolü
                return $"{where} {filter.Logic} ({string.Join(separator: $" {filter.Logic} ", value: filter.Filters.Select(f => Transform(f, filters)).ToArray())})";
            }

            return where.ToString();
        }
    }
}
