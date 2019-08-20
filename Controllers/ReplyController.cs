using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Data;
using Forum.Models.Reply;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers
{
    public class ReplyController : Controller
    {
        private readonly IForum _forumService;
        private readonly IPost _postService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPostReply _replyService;

        public ReplyController(IForum forumService, IPost postService, UserManager<ApplicationUser> userManager, IPostReply replyService)
        {
            _forumService = forumService;
            _postService = postService;
            _userManager = userManager;
            _replyService = replyService;
        }
        public async Task<IActionResult> Create(int id)
        {
            var post = _postService.GetById(id);
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var model = new PostReplyModel
            {
                PostContent = post.Content,
                PostTitle = post.Title,
                PostId = post.Id,

                AuthorName = User.Identity.Name,
                AuthorId = user.Id,

                ForumName = post.ForumModel.Title,
                ForumId = post.ForumModel.Id,

                Created = DateTime.Now
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddReply(PostReplyModel model)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);

            var reply = BuildReply(model, user);

            await _postService.AddReply(reply);

            return RedirectToAction("Index", "Post", new {id = model.PostId});
        }

        public async Task<IActionResult> Edit(int id)
        {
            var reply = _replyService.GetById(id);
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var model = new PostReplyModel
            {
                Id = reply.Id,
                ReplyContent = reply.Content,
                PostId = reply.Post.Id,

                AuthorName = User.Identity.Name,
                AuthorId = user.Id,

                Created = DateTime.Now
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditReply(PostReplyModel model)
        {

            await _replyService.Edit(model.Id, model.ReplyContent);

            return RedirectToAction("Index", "Post", new { id = model.PostId });
        }

        //private PostReply BuildEditReply(PostReplyModel model, ApplicationUser user)
        //{
        //    var post = _postService.GetById(model.PostId);

        //    return new PostReply
        //    {
        //        Id = model.Id,
        //        Post = post,
        //        Content = model.ReplyContent,
        //        Created = DateTime.Now,
        //        User = user
        //    };
        //}

        private PostReply BuildReply(PostReplyModel model, ApplicationUser user)
        {
            var post = _postService.GetById(model.PostId);

            return new PostReply
            {
                Post = post,
                Content = model.ReplyContent,
                Created = DateTime.Now,
                User = user
            };
        }
    }
}