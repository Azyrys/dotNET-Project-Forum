using dotNET_Project.Areas.Identity.Data;
using dotNET_Project.Data;
using dotNET_Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Policy;

namespace dotNET_Project.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly dotNET_ProjectContext _context;
        private string? _userId = "";
        private readonly bool isAdmin;

        public HomeController(ILogger<HomeController> logger, dotNET_ProjectContext context, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _context = context;

            if (httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) is not null)
            {
                _userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                if (_context.AllUsers.ToList().Find(user => user.Id == _userId).Permissions == "admin")
                {
                    isAdmin = true;
                }
                else
                {
                    isAdmin = false;
                }
            }
        }

        public IActionResult Index()
        {
            if (isAdmin)
                TempData["role"] = "admin";
            return View(_context.Posts.Include(t => t.Topic).Include(u => u.User).ToList());
        }

        public IActionResult Forbidden()
        {
            return View();
        }


        public ActionResult EditUser(string id)
        {
            if (isAdmin)
                TempData["role"] = "admin";
            if (_context.AllUsers.ToList().Find(x => x.Id == _userId) is not null && _context.AllUsers.ToList().Find(x => x.Id == _userId).Permissions == "admin")
            {
                return View(_context.AllUsers.Find(id));
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(string id, dotNET_ProjectUser user)
        {
            if (isAdmin)
                TempData["role"] = "admin";
            if (_context.AllUsers.ToList().Find(x => x.Id == _userId) is not null && _context.AllUsers.ToList().Find(x => x.Id == _userId).Permissions == "admin")
            {
                try
                {
                    dotNET_ProjectUser userToUpdate = _context.AllUsers.Find(id);
                    userToUpdate.Nickname = user.Nickname;
                    userToUpdate.Email = user.Email;
                    userToUpdate.Permissions = user.Permissions;
                    _context.SaveChanges();
                    return RedirectToAction(nameof(ManageUsers));
                }
                catch
                {
                    return View();
                }
            }
            else
            {
                return View();
            }

        }

        public IActionResult ManageUsers()
        {
            if (isAdmin)
                TempData["role"] = "admin";
            if (_context.AllUsers.ToList().Find(x => x.Id == _userId) is not null && _context.AllUsers.ToList().Find(x => x.Id == _userId).Permissions == "admin")
            {
                return View(_context.AllUsers.ToList());
            }
            else
            {
                return RedirectToAction(nameof(Forbidden));
            }
        }

        public ActionResult CreatePost()
        {
            if (isAdmin)
                TempData["role"] = "admin";
            if (_context.AllUsers.ToList().Find(x => x.Id == _userId) is not null)
            {
                var topics = _context.Topics.ToList();
                if (topics is null)
                {
                    topics = new List<Topic> { };
                }
                ViewBag.Topics = topics;
                return View();
            }
            else
            {
                return RedirectToAction(nameof(Forbidden));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePost(Post post)
        {
            if (isAdmin)
                TempData["role"] = "admin";
            if (_context.AllUsers.ToList().Find(x => x.Id == _userId) is not null)
            {
                post.Topic = _context.Topics.Find(post.TopicID);
                post.DateTime = DateTime.Now;
                post.User = _context.AllUsers.Find(_userId);
                _context.Posts.Add(post);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(Forbidden));
            }
        }
        public ActionResult DeletePost(int id)
        {
            if (isAdmin)
                TempData["role"] = "admin";
            if (_context.AllUsers.ToList().Find(x => x.Id == _userId) is not null && (_context.AllUsers.ToList().Find(x => x.Id == _userId).Permissions == "admin" || _context.Posts.ToList().Find(x => x.Id == id).User.Id == _userId))
            {
                return View(_context.Posts.Find(id));
            }
            else
            {
                return RedirectToAction(nameof(Forbidden));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id, Post post)
        {
            if (isAdmin)
                TempData["role"] = "admin";
            if (_context.AllUsers.ToList().Find(x => x.Id == _userId) is not null && (_context.AllUsers.ToList().Find(x => x.Id == _userId).Permissions == "admin" || _context.Posts.ToList().Find(x => x.Id == id).User.Id == _userId))
            {
                try
                {
                    Post postToDelete = _context.Posts.Find(id);
                    _context.Posts.Remove(postToDelete);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction(nameof(Forbidden));
            }
        }
        public ActionResult EditPost(int id)
        {
            if (isAdmin)
                TempData["role"] = "admin";
            if (_context.AllUsers.ToList().Find(x => x.Id == _userId) is not null && (_context.AllUsers.ToList().Find(x => x.Id == _userId).Permissions == "admin" || _context.Posts.ToList().Find(x => x.Id == id).User.Id == _userId))
            {
                return View(_context.Posts.Find(id));
            }
            else
            {
                return RedirectToAction(nameof(Forbidden));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int id, Post post)
        {
            if (isAdmin)
                TempData["role"] = "admin";
            if (_context.AllUsers.ToList().Find(x => x.Id == _userId) is not null && (_context.AllUsers.ToList().Find(x => x.Id == _userId).Permissions == "admin" || _context.Posts.ToList().Find(x => x.Id == id).User.Id == _userId))
            {
                try
                {
                    Post postToUpdate = _context.Posts.Find(id);
                    postToUpdate.Title = post.Title;
                    postToUpdate.Content = post.Content;
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction(nameof(Forbidden));
            }

        }
        public ActionResult ListComments(int id)
        {
            if (isAdmin)
                TempData["role"] = "admin";
            ViewBag.PostID = id;
            Post post = _context.Posts.Include(t => t.Topic).ToList().Find(p => p.Id == id);
            ViewBag.PostTitle = post.Title;
            ViewBag.PostContent = post.Content;
            ViewBag.Topic = post.Topic.Name;
            return View(_context.Comments.Include(u => u.User).Include(p => p.Post).Include(t => t.Post.Topic).ToList().Where(c => c.Post.Id == id));
        }

        public ActionResult CreateComment()
        {
            if (isAdmin)
                TempData["role"] = "admin";
            if (_context.AllUsers.ToList().Find(x => x.Id == _userId) is not null)
            {
                return View();
            }
            else
            {
                return RedirectToAction(nameof(Forbidden));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComment(Comment comment)
        {
            if (isAdmin)
                TempData["role"] = "admin";
            if (_context.AllUsers.ToList().Find(x => x.Id == _userId) is not null)
            {
                try
                {
                    comment.Post = _context.Posts.Find(comment.PostID);
                    comment.DateTime = DateTime.Now;
                    comment.User = _context.AllUsers.Find(_userId);
                    _context.Comments.Add(comment);
                    _context.SaveChanges();
                    return RedirectToAction("ListComments", new { id = comment.Post.Id });
                }
                catch
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction(nameof(Forbidden));
            }
        }
        public ActionResult DeleteComment(int id)
        {
            if (isAdmin)
                TempData["role"] = "admin";
            if (_context.AllUsers.ToList().Find(x => x.Id == _userId) is not null && (_context.AllUsers.ToList().Find(x => x.Id == _userId).Permissions == "admin" || _context.Posts.ToList().Find(x => x.Id == id).User.Id == _userId))
            {
                return View(_context.Comments.Find(id));
            }
            else
            {
                return RedirectToAction(nameof(Forbidden));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteComment(int id, Comment comment)
        {
            if (isAdmin)
                TempData["role"] = "admin";
            if (_context.AllUsers.ToList().Find(x => x.Id == _userId) is not null && (_context.AllUsers.ToList().Find(x => x.Id == _userId).Permissions == "admin" || _context.Posts.ToList().Find(x => x.Id == id).User.Id == _userId))
            {
                try
                {
                    int postId = _context.Comments.Include(x => x.Post).ToList().Find(x => x.Id == comment.Id).Post.Id;
                    Comment commentToDelete = _context.Comments.Find(id);
                    _context.Comments.Remove(commentToDelete);
                    _context.SaveChanges();
                    return RedirectToAction("ListComments", new { id = postId });
                }
                catch
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction(nameof(Forbidden));
            }
        }
        public ActionResult EditComment(int id)
        {
            if (isAdmin)
                TempData["role"] = "admin";
            if (_context.AllUsers.ToList().Find(x => x.Id == _userId) is not null && (_context.AllUsers.ToList().Find(x => x.Id == _userId).Permissions == "admin" || _context.Posts.ToList().Find(x => x.Id == id).User.Id == _userId))
            {
                return View(_context.Comments.Find(id));
            }
            else
            {
                return RedirectToAction(nameof(Forbidden));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditComment(int id, Comment comment)
        {
            if (isAdmin)
                TempData["role"] = "admin";
            if (_context.AllUsers.ToList().Find(x => x.Id == _userId) is not null && (_context.AllUsers.ToList().Find(x => x.Id == _userId).Permissions == "admin" || _context.Posts.ToList().Find(x => x.Id == id).User.Id == _userId))
            {
                try
                {
                    int postId = _context.Comments.Include(x => x.Post).ToList().Find(x => x.Id == comment.Id).Post.Id;
                    Comment commentToUpdate = _context.Comments.Find(id);
                    commentToUpdate.Content = comment.Content;
                    _context.SaveChanges();
                    return RedirectToAction("ListComments", new { id = postId });
                }
                catch
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction(nameof(Forbidden));
            }

        }

        public ActionResult CreateTopic()
        {
            if (isAdmin)
                TempData["role"] = "admin";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTopic(Topic topic)
        {
            if (isAdmin)
                TempData["role"] = "admin";
            try
            {
                if (_context.Topics.ToList().Find(x => x.Name == topic.Name) is null)
                {
                    _context.Topics.Add(topic);
                    _context.SaveChanges();
                }
                return RedirectToAction(nameof(ManageTopics));
            }
            catch
            {
                return View();
            }
        }

        public IActionResult ManageTopics()
        {
            if (isAdmin)
                TempData["role"] = "admin";
            if (_context.AllUsers.ToList().Find(x => x.Id == _userId) is not null && _context.AllUsers.ToList().Find(x => x.Id == _userId).Permissions == "admin")
            {
                return View(_context.Topics.ToList());
            }
            else
            {
                return RedirectToAction(nameof(Forbidden));
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            if (isAdmin)
                TempData["role"] = "admin";
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}