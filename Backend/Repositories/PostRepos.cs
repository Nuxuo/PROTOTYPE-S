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

        public IEnumerable<Post> GetPostsByUser(Guid _Id){
            return _context.Posts.Where(x=>x.Poster.Id == _Id).ToList();
        }

        public Post GetPostById(Guid _Id){
            return _context.Posts
                .Include(x=>x.Comments)
                .Include(x=>x.Poster)
                .Include(x => x.Tags)
                    .ThenInclude(x => x.Tag)
            .FirstOrDefault(x => x.Id == _Id);
        }

        public IEnumerable<Comment> GetPostsComments(Guid _Id){
            return _context.Comments.Where(x => x.PostId == _Id).ToList();
        }

        // POST
        public Post CreatePost(PostEntryDto _input){
            List<PostTag> _postTagsList = new List<PostTag>();
            Post _post = new Post(_input);
            _context.Posts.Add(_post);
            _context.SaveChanges();

            foreach(string _tagEntry in _input.Tags){
                Tag _tag = _context.Tags.FirstOrDefault(x=>x.Name == _tagEntry);
                Guid _TagGuid;

                if(_tag != null){
                    _TagGuid = _tag.Id;
                }
                else{
                    Tag _NewTag = new Tag{
                        Name = _tagEntry
                    } ;

                    _context.Tags.Add(_NewTag);
                    _context.SaveChanges();

                    _TagGuid = _NewTag.Id;

                }

                _context.PostTags.Add(new PostTag{
                    PostId = _post.Id,
                    TagId = _TagGuid
                });

                _context.SaveChanges();
            }
            
            return _post;
        }


        // PUT
        public Post UpdatePost(Guid _Id, PostEntryDto _input){
            var _post = _context.Posts.Include(x => x.Poster).FirstOrDefault(x => x.Id == _Id);

            _post.HeadLine = _input.Headline;
            _post.Content = _input.Content;
            _post.UpdatedDate = DateTime.UtcNow;
            _context.SaveChanges();
            return _post;
        }
    }
}