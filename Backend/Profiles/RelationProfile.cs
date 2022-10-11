using AutoMapper;
using Models;
using DataTransferObject;

namespace Profiles{
    public class RelationProfile : Profile
        {
            public RelationProfile()
            {
                CreateMap<UserPostRelation, UserPostRelationDto>();
                CreateMap<UserCommentRelation, UserCommentRelationDto>();
            }
        }
}