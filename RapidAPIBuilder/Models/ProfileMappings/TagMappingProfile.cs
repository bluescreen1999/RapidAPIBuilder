using AutoMapper;
using RapidAPIBuilder.Models.Dtos.Tags;
using RapidAPIBuilder.Models.Entties;

namespace RapidAPIBuilder.Models.ProfileMappings;

public class TagMappingProfile : Profile
{
    public TagMappingProfile()
    {
        CreateMap<CreateTagRequest, Tag>();
        CreateMap<Tag, CreateTagResponse>();
        CreateMap<Tag, GetTagDetailsResponse>();
        CreateMap<Tag, GetAllTagsResponse>();
        CreateMap<UpdateTagReqest, Tag>();
    }
}
