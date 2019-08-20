using System;
using Forum.Data;
using Forum.Models;
using Forum.Models.Forum;
using Forum.Models.Post;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Forum.Service;
using Microsoft.AspNetCore.Identity;

namespace Forum.Controllers
{
    public class HomeController : Controller
    {

        //private HomeIndexModel BuildHomeIndexModel()
        //{
        //    throw new NotImplementedException();
        //}

        

        private readonly IForum _forumService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;

        public HomeController(IForum forumService, UserManager<ApplicationUser> userManager, IEmailService emailService)
        {
            _forumService = forumService;
            _userManager = userManager;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            var forums = _forumService.GetAll()
                .Select(forum => new ForumListingModel
                {
                    Id = forum.Id,
                    Name = forum.Title,
                    Description = forum.Description
                });

            var user = _userManager.FindByNameAsync(User.Identity.Name);
            ForumIndexModel model;
    
            try
            {
                model = new ForumIndexModel
                {
                    ForumList = forums,
                    User = user.Result
                };
            }
            catch (Exception e)
            {
                model = new ForumIndexModel
                {
                    ForumList = forums
                };
            }
            

            return View(model);
        }
        public IActionResult Privacy()
        {
            return View();
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

        public async Task<IActionResult> Verification(ApplicationUser user)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = user.Id, code = code },
                protocol: Request.Scheme);


            await _emailService.SendEmailAsync(user.Email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            return View();
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


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}




