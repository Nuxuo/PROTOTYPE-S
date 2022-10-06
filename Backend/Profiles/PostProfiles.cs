using AutoMapper;
using Models;
using DataTransferObject;

namespace Profiles{
    public class PostProfiles : Profile
        {
            public PostProfiles()
            {
                CreateMap<Post, PostDto>();
                CreateMap<Post, PostEntryDto>();
                CreateMap<Post, PostSimpleDto>();
                CreateMap<Post, PostRatingDto>();
                CreateMap<Post, PostSimpleDto>();
                CreateMap<PostRatingDto, PostDto>();
                CreateMap<PostRatingDto, PostSimpleDto>();
                CreateMap<Comment, CommentSimpleDto>();
            }
        }

}