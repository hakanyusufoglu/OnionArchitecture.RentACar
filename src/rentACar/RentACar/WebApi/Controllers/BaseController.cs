using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class BaseController:ControllerBase
    {
        //Daha önce mediator inject edildiyse onu döndür yoksa ıoc kontrolünde service olarak al diyoruz.
        //Protecterd olduğu için Mediator ismi yaptık
        private IMediator? _mediator;
        protected IMediator? Mediator => _mediator??= HttpContext.RequestServices.GetService<IMediator>(); //bunu sadece inharte eden kullansın diye protected
    }
}
