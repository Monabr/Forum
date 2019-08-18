using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Data;
using Microsoft.EntityFrameworkCore;

namespace Forum.Service
{
    public class ForumService:IForum
    {
        private readonly ApplicationDbContext _context;
        public ForumService(ApplicationDbContext context)
        {
            _context = context;
        }
        public ForumModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ForumModel> GetAll()
        {
            return _context.Forums
                .Include(forum=>forum.Posts);
        }

        public IEnumerable<ApplicationUser> ApplicationUsers()
        {
            throw new NotImplementedException();
        }

        public Task Create(ForumModel forum)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int forumId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateForumTitle(int forumId, string newTitle)
        {
            throw new NotImplementedException();
        }

        public Task UpdateForumDescription(int forumId, string newDescription)
        {
            throw new NotImplementedException();
        }
    }
}
