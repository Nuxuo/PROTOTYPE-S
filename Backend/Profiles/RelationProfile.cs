using AutoMapper;
using Models;
using DataTransferObject;

namespace Profiles{
    public class RelationProfile : Profile
        {
            public RelationProfile()
            {
                // CreateMap<UserPostRelation, UserPostRelationdDto>()
                //     .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Liked == true ? "Liked!" : "Disliked.."));
                CreateMap<UserPostRelation, UserPostRelationDto>();
                CreateMap<UserCommentRelation, UserCommentRelationDto>();
            }
        }
}