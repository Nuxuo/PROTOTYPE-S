using AutoMapper;
using Models;
using DataTransferObject;

namespace Profiles{
    public class UserProfiles : Profile
        {
            public UserProfiles()
            {
                // CreateMap<User, UserDto>()
                //     .ForMember(c => c.PostsLiked, m => m.MapFrom(l => _UserPostRelation(l.UserPostRelation,true)))
                //     .ForMember(c => c.PostsDisliked, m => m.MapFrom(l => _UserPostRelation(l.UserPostRelation,false)))
                //     .ForMember(c => c.CommentsDisliked, m => m.MapFrom(l => _UserCommentRelation(l.UserCommentRelations,false)))
                //     .ForMember(c => c.CommentsLiked, m => m.MapFrom(l => _UserCommentRelation(l.UserCommentRelations,true)));

                CreateMap<User, UserDto>();
                CreateMap<User, UserSimpleDto>();
                CreateMap<UserEntryDto, User>();
                CreateMap<Comment, CommentSimplePostDto>();
                CreateMap<Post, PostEntryDto>();  

            }
            public List<Guid> _UserPostRelation(ICollection<UserPostRelation> _List , bool _s){
                List<Guid> returnList = new List<Guid>();
                foreach(UserPostRelation _t in _List){
                    if(_t.Liked == _s)
                        returnList.Add(_t.PostGuid);
                }
                return returnList;
            }

            public List<Guid> _UserCommentRelation(ICollection<UserCommentRelation> _List , bool _s){
                List<Guid> returnList = new List<Guid>();
                foreach(UserCommentRelation _t in _List){
                    if(_t.Liked == _s)
                        returnList.Add(_t.CommentGuid);
                }
                return returnList;
            }
        }
}