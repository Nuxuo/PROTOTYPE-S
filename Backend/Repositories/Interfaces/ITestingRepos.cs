using Models;
using DataTransferObject;

namespace Repositories{
    public interface ITestingRepos{
        public Task<bool> CreatePosts(int ammount);
        public Task<bool> CreateComments(int ammount);
        public Task<bool> UserRelationPosts(int ammount);
        public Task<bool> UserRelationComments(int ammount);

        public Task<bool> TruncateData();

    }
}