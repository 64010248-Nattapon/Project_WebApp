using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using System.Net;

namespace backend.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly PostContext _context;


        public PostController(PostContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPostAll()
        {
            if (_context.Posts == null)
            {
                return NotFound();
            }
            return await _context.Posts.ToListAsync();
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<Post>>> Index(string status)
        {
            if (_context.Posts == null)
            {
                return Problem("Entity set 'PostContext.Posts'  is null.");
            }

            var recieves = from m in _context.Posts
                           select m;

            if (!String.IsNullOrEmpty(status))
            {
                recieves = recieves.Where(x => x.status == status);
            }

            return await recieves.ToListAsync();
        }

        [HttpGet("user/{user}")]
        public async Task<ActionResult<IEnumerable<Post>>> IndexUser(string user)
        {
            if (_context.Posts == null)
            {
                return Problem("Entity set 'PostContext.Posts'  is null.");
            }

            var recieves = from m in _context.Posts
                           select m;

            if (!String.IsNullOrEmpty(user))
            {
                recieves = recieves.Where(x => x.username == user);
            }

            return await recieves.ToListAsync();
        }


        [HttpGet("{postId}")]
        public async Task<ActionResult<IEnumerable<Post>>> getPostByPostId(long postId)
        {
            if (_context.Posts == null)
            {
                return Problem("Entity set 'PostContext.Posts'  is null.");
            }

            var recieves = from m in _context.Posts
                           select m;
            recieves = recieves.Where(x => x.postId == postId);
            return await recieves.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Post>> CreatePost(PostCreateDTO post)
        {
            if (_context.Posts == null)
            {
                return Problem("Entity set 'PostContext.PostModels'  is null.");
            }
            var newPost = new Post
            {
                postId = post.postId,
                username = post.username,
                nickname = post.nickname,
                realname = post.realname,
                storename = post.storename,
                amount = post.amount,
                location = post.location,
                reserved = post.reserved,
                date = post.date,
                timeCreated = DateTime.Now,
                status = "กำลังรับออเดอร์",
            };
            _context.Posts.Add(newPost);
            await _context.SaveChangesAsync();
            return CreatedAtAction("CreatePost", new { id = post.postId }, post);
        }

        [HttpPut("{postId}")]
        public async Task<ActionResult<Post>> PutStatus(PostUpdateStatus request)
        {
            if (_context.Posts == null)
            {
                return Problem("Entity set 'PostContext.PostModels'  is null.");
            }
            if (!PostExists(request.postId))
            {
                return NotFound();
            }
            var selectPost = _context.Posts.Where(e => e.postId == request.postId).FirstOrDefault();
            if (selectPost == null)
            {
                return NotFound();
            }
            selectPost.status = request.status;
            _context.Update(selectPost);
            await _context.SaveChangesAsync();
            return Ok("post Update");
        }

        private bool PostExists(long? id)
        {
            return (_context.Posts?.Any(e => e.postId == id)).GetValueOrDefault();
        }

    }
}