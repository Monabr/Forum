using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Data
{
    public interface IForum
    {
        ForumModel GetById(int id);
        IEnumerable<ForumModel> GetAll();
        IEnumerable<ApplicationUser> ApplicationUsers();

        Task Create(ForumModel forum);
        Task Delete(int forumId);
        Task UpdateForumTitle(int forumId, string newTitle);
        Task UpdateForumDescription(int forumId, string newDescription);
    }
}
