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
        public Post CreatePost(PostEntryDto _entry){
            List<PostTag> _postTagsList = new List<PostTag>();
            Post _post = new Post(_entry);
            _context.Posts.Add(_post);
            _context.SaveChanges();

            foreach(string _tagEntry in _entry.Tags){
                Tag _tag = _context.Tags.FirstOrDefault(x=>x.Name == _tagEntry);
                Guid _TagId;

                if(_tag != null){
                    _TagId = _tag.Id;
                }
                else{
                    Tag _NewTag = new Tag{
                        Name = _tagEntry
                    } ;

                    _context.Tags.Add(_NewTag);
                    _context.SaveChanges();

                    _TagId = _NewTag.Id;

                }

                _context.PostTags.Add(new PostTag{
                    PostId = _post.Id,
                    TagId = _TagId
                });

                _context.SaveChanges();
            }
            
            return _post;
        }
        public Post UpdatePost(Guid _Id, PostEntryDto _entry){
            var _post = _context.Posts.Include(x => x.Poster).FirstOrDefault(x => x.Id == _Id);

            _post.HeadLine = _entry.Headline;
            _post.Content = _entry.Content;
            _post.UpdatedDate = DateTime.UtcNow;
            _context.SaveChanges();
            return _post;
        }
    }
}