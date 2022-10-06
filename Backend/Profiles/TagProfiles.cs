using AutoMapper;
using Models;
using DataTransferObject;

namespace Profiles{
    public class TagProfiles : Profile
        {
            public TagProfiles()
            {
                CreateMap<UserTag, TagLikesDto>()
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Tag.Name)); 

                CreateMap<PostTag, TagDto>()
                  .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Tag.Name));  

                CreateMap<PostSimpleDto, TagDto>();

                CreateMap<PostTag, PostSimpleDto>();
            }
        }
}