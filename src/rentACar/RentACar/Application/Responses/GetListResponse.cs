using Core.Persistence.Paging;

namespace Application.Responses
{
    //hem itemler hem sayfalamla ilgil verilerim oldu
    public class GetListResponse<T> : BasePageableModel
    {
        private IList<T> _items;
        public IList<T> Items
        {
            get => _items ??= new List<T>(); //items yok ise yeni bir list oluştur
            set => _items = value;
        }
    }
}
