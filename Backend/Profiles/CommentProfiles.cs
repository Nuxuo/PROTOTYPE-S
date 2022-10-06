using AutoMapper;
using Models;
using DataTransferObject;

namespace Profiles{
    public class CommentProfiles : Profile
        {
            public CommentProfiles()
            {
                CreateMap<Comment, CommentDto>();
                CreateMap<Comment, CommentSimpleDto>();
                CreateMap<Comment, CommentSimpleUserDto>();
                CreateMap<Comment, CommentSimplePostDto>();
            }
        }

}