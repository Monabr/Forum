using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Data;
using Microsoft.EntityFrameworkCore;

namespace Forum.Service
{
    public class PostReplyService: IPostReply
    {
        private readonly ApplicationDbContext _context;

        public PostReplyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public PostReply GetById(int id)
        {
            return _context.PostReplies
                .Include(r => r.Post)
                .ThenInclude(post => post.ForumModel)
                .Include(r => r.Post)
                .ThenInclude(post => post.User).First(r => r.Id == id);
        }

        public async Task Edit(int id, string massage)
        {
            var reply = GetById(id);
            reply.Content = massage;
            _context.Update(reply);
            await _context.SaveChangesAsync();
        }
    }
}
