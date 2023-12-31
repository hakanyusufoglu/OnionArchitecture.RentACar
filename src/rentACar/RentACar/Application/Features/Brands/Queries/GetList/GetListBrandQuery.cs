﻿using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Caching;
using Core.Application.Pipelines.Logging;
using Core.Application.Request;
using Core.Application.Responses;
using Core.Persistence.Paging;
using Domain.Entities;
using MediatR;

namespace Application.Features.Brands.Queries.GetList
{
    //GetListBrandQuery querysinin geri dönüş responsu GetListResponse bu olacaktır.
    public class GetListBrandQuery:IRequest<GetListResponse<GetListBrandListItemDto>>, ICachableRequest, ILoggableRequest
    {
        public PageRequest PageRequest { get; set; }

        //Yapılacak işin ismi olabilir ve key unique olmalı. Sayfa bazlı da cache olacağı için PageRequest verdik
        public string CacheKey => $"GetListBrandQuery({PageRequest.PageIndex},{PageRequest.PageSize})";

        public bool BypassCache { get; }

        public TimeSpan? SlidingExpiration { get; }

        public string? CacheGroupKey => "GetBrands";

        //CQRS
        //GetListBrandQuery querysinin geri dönüş responsu GetListResponse bu olacaktır.
        public class GetListBrandQueryHandler : IRequestHandler<GetListBrandQuery,GetListResponse<GetListBrandListItemDto>>
        {
            private readonly IBrandRepository _brandRepository;
            private readonly IMapper _mapper;

            public GetListBrandQueryHandler(IBrandRepository brandRepository, IMapper mapper)
            {
                _brandRepository = brandRepository;
                _mapper = mapper;
            }

            public async Task<GetListResponse<GetListBrandListItemDto>> Handle(GetListBrandQuery request, CancellationToken cancellationToken)
            {
                //request demek GetListBrandQuery nesnesine karşılık gelmektedir. request = GetListBrandQuery
               Paginate<Brand> brands = await _brandRepository.GetListAsync(
                    index:request.PageRequest.PageIndex,
                    size:request.PageRequest.PageSize,
                    cancellationToken:cancellationToken
                    );
                GetListResponse<GetListBrandListItemDto> response = _mapper.Map<GetListResponse<GetListBrandListItemDto>>(brands);
                return response;
            }
        }
    }
}
