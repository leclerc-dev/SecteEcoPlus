using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecteEcoPlus.Areas.Identity;
using SecteEcoPlus.Areas.Identity.Data;
using SecteEcoPlus.Models;
using SecteEcoPlus.ViewModels;

namespace SecteEcoPlus.Controllers
{
    [ApiController]
    public class ReviewController : Controller
    {
        private WebsiteContext _context;
        private SecteUserManager _userManager;
        private SignInManager<SecteUser> _signInManager;

        public ReviewController(WebsiteContext context, SecteUserManager userManager, SignInManager<SecteUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpPost, Route("api/submit-message")]
        public async Task<IActionResult> Submit([FromForm] MessageRequestViewModel messageRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            PublicProfile profile = null;
            if (!messageRequest.IsAnonymous && _signInManager.IsSignedIn(User))
            {
                profile = _context
                          .Users
                          .Select(u => u.PublicProfile)
                          .FirstOrDefault(p => p.SecteUserId == _userManager.GetUserId(User));
            }
            var model = new Message
            {
                Author = profile,
                Content = messageRequest.Content,
                PublishDate = DateTime.Now
            };
            await _context.WebsiteReviews.AddAsync(model);
            await _context.SaveChangesAsync();
            ViewBag.IsRecent = true;
            return PartialView("_ReviewPartial", model);
        }

        [HttpGet, Route("api/delete-message/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return Unauthorized();
            }
            var user = await _userManager.GetPublicProfileByUserAsync(User);
            if (user is null)
            {
                return BadRequest();
            }
            var message = await _context.WebsiteReviews.FindAsync(id);
            if (message is null)
            {
                return NotFound();
            }

            if (message.AuthorId != user.PublicProfileId)
            {
                return Unauthorized();
            }
            // alright everything is fine
            _context.WebsiteReviews.Remove(message);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}