using Application.Features.Brands.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Caching;
using Core.Application.Pipelines.Transaction;
using Domain.Entities;
using MediatR;

namespace Application.Features.Brands.Commonds.Create
{
    //Sen bir requestsin/ Apiden böyle bir command gelecek bizde çevirip db'ye kaydediceğiz.
    public class CreateBrandCommand : IRequest<CreatedBrandResponse>, ITransactionalRequest, ICacheRemoverRequest //aslında brand nesnesi almıyoruz da sadece command alıyoruz ve transactional bir requestsin diyoruz.
    {
        //Bana bir brand command requesti gelecek ben de CreatedBrandReponse döndüreceğim.
        public string Name { get; set; }

        public string CacheKey => ""; //tek bir cache anahtarımız olmadığı için bunu kuyllanmıyoruz aslında
        public bool BypassCache => false;
        public string? CacheGroupKey => "GetBrands";

        public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, CreatedBrandResponse> // Sen bir requesthandlersin kimin ? CreateBrandCommand sınıfının
        {
            private readonly IBrandRepository _brandRepository;
            private readonly IMapper _mapper;
            private readonly BrandBusinessRules _brandBusinessRules;
            public CreateBrandCommandHandler(IBrandRepository brandRepository, IMapper mapper, BrandBusinessRules brandBusinessRules)
            {
                _brandRepository = brandRepository;
                _mapper = mapper;
                _brandBusinessRules = brandBusinessRules;
            }

            //CreateBrandCommand, commendi geldiğinde CreateBrandCommandHandler çalışacaktır.
            public async Task<CreatedBrandResponse>? Handle(CreateBrandCommand request, CancellationToken cancellationToken)
            {

                //Aynı name'den veri eklendiğinde bu sınıftaki hata sınıfındaki throw new BrandBusinessRules fırlatılacaktır.
                await _brandBusinessRules.BrandNameCannotBeDuplicatedWhenInserted(request.Name);

                //Request'i brande çevir
                Brand brand = _mapper.Map<Brand>(request);
                brand.Id = Guid.NewGuid();

                await _brandRepository.AddAsync(brand);

                //Aynı nesneyi bilerek döndürmüyoruz çünkü veri güvenliğini sağladık. Örneğin Veri tabanında 100 alan varsa 5'iyle çalıştığımızdan gerekli olan tabloları döndürmek için.
                CreatedBrandResponse createdBrandResponse = _mapper.Map<CreatedBrandResponse>(brand); //brand nesnesi referens tipli olduğu için direkt olarak buraya verebilirim.
                return createdBrandResponse;
            }
        }
    }
}
