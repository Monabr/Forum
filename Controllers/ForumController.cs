using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Data;
using Forum.Models.Forum;
using Forum.Models.Post;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers
{
    public class ForumController : Controller
    {
        private readonly IForum _forumService;
        private readonly IPost _postService;
        public ForumController(IForum forumService)
        {
            _forumService = forumService;
        }
        public IActionResult Index()
        {
            var forums = _forumService.GetAll()
                .Select(forum => new ForumListingModel {
                    Id = forum.Id,
                    Name = forum.Title,
                    Description = forum.Description
            });

            var model = new ForumIndexModel
            {
                ForumList = forums
            };

            return View(model);
        }

        public IActionResult Topic(int id)
        {
            var forum = _forumService.GetById(id);
            var posts = forum.Posts;

            var postListings = posts.Select(post => new PostListingModel
            {
                Id = post.Id,
                AuthorId = post.User.Id,
                AuthorName = post.User.UserName,
                Title = post.Title,
                DatePosted = post.Created.ToString(),
                RepliesCount = post.Replies.Count(),
                Forum = BuildForumListing(post)
            });

            var model = new ForumTopicModel
            {
                Posts = postListings,
                Forum = BuildForumListing(forum)
            };

            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddForum(AddForumModel model)
        {

            var forum = new ForumModel
            {
                Title = model.Title,
                Description = model.Description,
                Created = DateTime.Now
            };

            await _forumService.Create(forum);
            return RedirectToAction("Index", "Home");
        }

        private ForumListingModel BuildForumListing(Post post)
        {
            var forum = post.ForumModel;
            return BuildForumListing(forum);
        }

        private ForumListingModel BuildForumListing(ForumModel forum)
        {
            return new ForumListingModel
            {
                Id = forum.Id,
                Name = forum.Title,
                Description = forum.Description
            };
        }
    }
}