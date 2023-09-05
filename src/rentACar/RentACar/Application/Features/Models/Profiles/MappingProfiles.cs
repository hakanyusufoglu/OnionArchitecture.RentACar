using Application.Features.Models.Queries.GetList;
using Application.Features.Models.Queries.GetListByDynamic;
using Application.Responses;
using AutoMapper;
using Core.Persistence.Paging;
using Domain.Entities;

namespace Application.Features.Models.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Model, GetListModelListItemDto>()
                .ForMember(destinationMember: c => c.BrandName, memberOptions: opt => opt.MapFrom(c => c.Name))
                .ForMember(destinationMember: c => c.FuelName, memberOptions: opt => opt.MapFrom(c => c.Name))
                .ForMember(destinationMember: c => c.TransmissionName, memberOptions: opt => opt.MapFrom(c => c.Transmission.Name))
                .ReverseMap();
            CreateMap<Paginate<Model>, GetListResponse<GetListModelListItemDto>>().ReverseMap();

            CreateMap<Model, GetListByDynamicModelListItemDto>()
            .ForMember(destinationMember: c => c.BrandName, memberOptions: opt => opt.MapFrom(c => c.Name))
            .ForMember(destinationMember: c => c.FuelName, memberOptions: opt => opt.MapFrom(c => c.Name))
            .ForMember(destinationMember: c => c.TransmissionName, memberOptions: opt => opt.MapFrom(c => c.Transmission.Name))
            .ReverseMap();
            CreateMap<Paginate<Model>, GetListResponse<GetListByDynamicModelListItemDto>>().ReverseMap();
        }
    }
}
