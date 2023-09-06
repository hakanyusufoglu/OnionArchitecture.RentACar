using MediatR;
using System.ComponentModel.Design;
using System.Transactions;

namespace Core.Application.Pipelines.Transaction
{
    //ITransactionalRequest inherit eden her sınıf bunu kullanabilsin diyoruz.
    //Bu yapı sayesinde örneğin Brand tablosuna aynı değerde BMW değeri eklensin ilk transaction da BMW eklenecektir ancak hemen ardından tekrar veri tabanına ekleme komutu verilirse ve BMW değeri eklenirse ikinci transaction validationa takılacağından bu iki transactionı geri alacaktır.
    public class TransactionScopeBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>, ITransactionalRequest
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            using TransactionScope transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
             TResponse response;
            try //transaction başarılı
            {
                response = await next();
                transactionScope.Complete();
            }
            catch (Exception) //transaction başarısız ve geri al
            {
                transactionScope.Dispose();
                throw;
            }
            return response;
        }
    }
}
