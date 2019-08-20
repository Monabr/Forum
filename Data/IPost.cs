using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Data
{
    public interface IPost
    {
        Post GetById(int id);

        Task Add(Post post);
        Task EditPostContent(int id, string newContent);

        Task AddReply(PostReply reply);

    }
}
