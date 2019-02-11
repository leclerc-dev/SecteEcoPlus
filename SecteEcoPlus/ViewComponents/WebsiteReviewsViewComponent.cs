using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecteEcoPlus.Models;

namespace SecteEcoPlus.ViewComponents
{
    public class WebsiteReviewsViewComponent : ViewComponent
    {
        private WebsiteContext _context;
        public WebsiteReviewsViewComponent(WebsiteContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(IEnumerable<Message> baseMessages = null)
        {
            var messages = baseMessages ?? await _context.WebsiteReviews
                .AsNoTracking()
                .Include(m => m.Author)
                .OrderByDescending(x => x.PublishDate)
                .Take(10)
                .ToListAsync(); // last 10
            return View(messages);
        }
    }
}