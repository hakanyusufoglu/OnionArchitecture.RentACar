using Application.Features.Brands.Constants;
using Application.Services.Repositories;
using Core.Application.Rules;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Domain.Entities;

namespace Application.Features.Brands.Rules
{
    //BaseBusinessRules ile sadece imzaladık.
    public class BrandBusinessRules:BaseBusinessRules
    {
        private readonly IBrandRepository _brandRepository;

        public BrandBusinessRules(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }
        //Marka ismi marka eklerken tekrar edemez
        public async Task BrandNameCannotBeDuplicatedWhenInserted(string name)
        {
            Brand? result = await _brandRepository.GetAsync(predicate:b=>b.Name.ToLower()== name.ToLower());
            if (result !=null) 
            {
                //Hata gruplaması yapıldı.
                throw new BusinessException(BrandsMessages.BrandNameExists);
            }
        }
    }
}
