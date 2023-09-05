using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Brands.Commonds.Update
{
    public class UpdateBrandCommand : IRequest<UpdatedBrandResponse>
    {

        //Command'in içerisinde bu yapılan istekte gerekli olan bilgiler ekleniyor. 
        public Guid Id { get; set; }
        public string Name { get; set; }

        public class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand, UpdatedBrandResponse>
        {
            private readonly IBrandRepository _brandRepository;
            private readonly IMapper _mapper;

            public UpdateBrandCommandHandler(IBrandRepository brandRepository, IMapper mapper)
            {
                _brandRepository = brandRepository;
                _mapper = mapper;
            }

            public async Task<UpdatedBrandResponse> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
            {
                Brand? brand = await _brandRepository.GetAsync(predicate: b => b.Id == request.Id, cancellationToken: cancellationToken);
                //ToDo: İş kuralı olmalı böyle bir ıd'ye sahip data var mı diye
                brand = _mapper.Map(request, brand); //request'i branda taşır
                await _brandRepository.UpdateAsync(brand);
                UpdatedBrandResponse response = _mapper.Map<UpdatedBrandResponse>(brand);
                return response;

            }
        }
    }
}
