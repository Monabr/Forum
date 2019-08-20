using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Data;
using Forum.Models.Post;
using Forum.Models.Reply;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers
{
    public class PostController : Controller
    {
        private readonly IPost _postService;
        private readonly IForum _forumService;

        private static UserManager<ApplicationUser> _userManager;

        public PostController(IPost postService, IForum forumService, UserManager<ApplicationUser> userManager)
        {
            _postService = postService;
            _forumService = forumService;
            _userManager = userManager;
        }
        public IActionResult Index(int id)
        {
            var post = _postService.GetById(id);
            var replies = BuildPostReplies(post.Replies);

            var model = new PostIndexModel
            {
                Id = post.Id, 
                Title = post.Title,
                AuthorId = post.User.Id,
                AuthorName = post.User.UserName,
                Created = post.Created,
                PostContent = post.Content,
                Replies = replies
            };

            return View(model);
        }

        public IActionResult Create(int id)
        {
            var forum = _forumService.GetById(id);

            var model = new NewPostModel
            {
                ForumName = forum.Title,
                ForumId = forum.Id,
                AuthorName = User.Identity.Name
            };

            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var post = _postService.GetById(id);

            var model = new PostIndexModel
            {
                PostContent = post.Content
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddPost(NewPostModel model)
        {
            var userId = _userManager.GetUserId(User);
            var user = _userManager.FindByIdAsync(userId).Result;
            var post = BuildPost(model, user);

            _postService.Add(post).Wait();

            return RedirectToAction("Index", "Post", new {id = post.Id});
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(PostIndexModel model)
        {
            _postService.EditPostContent(model.Id, model.PostContent).Wait();

            return RedirectToAction("Index", "Post", new { id = model.Id });
        }

        private Post BuildPost(NewPostModel model, ApplicationUser user)
        {
            var forum = _forumService.GetById(model.ForumId);

            return new Post
            {
                Title = model.Title,
                Content = model.Content,
                Created = DateTime.Now,
                User = user,
                ForumModel = forum
            };
        }

        private IEnumerable<PostReplyModel> BuildPostReplies(IEnumerable<PostReply> replies)
        {
            return replies.Select(reply => new PostReplyModel
            {
                Id = reply.Id,
                AuthorName = reply.User.UserName,
                AuthorId = reply.User.Id,
                Created = reply.Created,
                ReplyContent = reply.Content
            });
        }
    }
}