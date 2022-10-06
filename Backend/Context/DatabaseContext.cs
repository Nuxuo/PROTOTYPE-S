using Microsoft.EntityFrameworkCore;
using Models;

namespace Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {}
        
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Post> Posts { get; set; }

        public DbSet<Tag> Tags { get; set; }
        public DbSet<UserTag> UserTags { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        public DbSet<UserPostRelation> UserPostRelations { get; set; }
        public DbSet<UserCommentRelation> UserCommentRelations { get; set; }
    }
}