using Microsoft.EntityFrameworkCore;
using Context;
using DataTransferObject;
using Models;

namespace Repositories{
    public class PostRepos : BaseRepos, IPostRepos{
        public PostRepos(DatabaseContext context){
            _context = context ?? throw new NullReferenceException(nameof(context));
        }
        public IEnumerable<Post> GetPosts(){
            return _context.Posts                
                .Include(x => x.Tags)
                    .ThenInclude(x => x.Tag)
            .ToList();
        }

        public IEnumerable<Post> GetPostsByUser(Guid _guid){
            return _context.Posts.Where(x=>x.Poster.guId == _guid).ToList();
        }

        public Post GetPostById(Guid _guid){
            return _context.Posts
                .Include(x=>x.Comments)
                .Include(x=>x.Poster)
                .Include(x => x.Tags)
                    .ThenInclude(x => x.Tag)
            .FirstOrDefault(x => x.guId == _guid);
        }

        public IEnumerable<Comment> GetPostsComments(Guid _guid){
            return _context.Comments.Where(x => x.PostGuid == _guid).ToList();
        }

        // POST
        public Post CreatePost(PostEntryDto _input){
            List<PostTag> _postTagsList = new List<PostTag>();
            Post _post = new Post{
                UserGuid = _input.UserGuid,
                HeadLine = _input.Headline,
                Content = _input.Content
            };
            _context.Posts.Add(_post);
            _context.SaveChanges();

            foreach(string _tagEntry in _input.Tags){
                Tag _tag = _context.Tags.FirstOrDefault(x=>x.Name == _tagEntry);
                Guid _TagGuid;

                if(_tag != null){
                    _TagGuid = _tag.guId;
                }
                else{
                    Tag _NewTag = new Tag{
                        Name = _tagEntry
                    } ;

                    _context.Tags.Add(_NewTag);
                    _context.SaveChanges();

                    _TagGuid = _NewTag.guId;

                }

                _context.PostTags.Add(new PostTag{
                    PostGuid = _post.guId,
                    TagGuid = _TagGuid
                });

                _context.SaveChanges();
            }
            
            return _post;
        }


        // PUT
        public Post UpdatePost(Guid _guid, PostEntryDto _input){
            var _post = _context.Posts.Include(x => x.Poster).FirstOrDefault(x => x.guId == _guid);

            _post.HeadLine = _input.Headline;
            _post.Content = _input.Content;
            _post.UpdatedDate = DateTime.UtcNow;
            _context.SaveChanges();
            return _post;
        }
    }
}