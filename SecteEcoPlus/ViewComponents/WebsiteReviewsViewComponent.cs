using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SecteEcoPlus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SecteEcoPlus.Cache;
namespace SecteEcoPlus.ViewComponents
{
    public class WebsiteReviewsViewComponent : ViewComponent
    {
        private readonly WebsiteContext _context;
        private readonly IMemoryCache _cache;
        public WebsiteReviewsViewComponent(WebsiteContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }
        public async Task<IViewComponentResult> InvokeAsync(IEnumerable<Message> baseMessages = null)
        {
            var messages = baseMessages?.ToList() ?? await _cache.GetOrCreateAsync(LastReviews, async i =>
            {
                i.SetOptions(new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(12)));
                return await _context.WebsiteReviews
                    .AsNoTracking()
                    .Include(m => m.Author)
                    .OrderByDescending(x => x.PublishDate)
                    .Take(10)
                    .ToListAsync();
            });
            return View(messages);
        }
    }
}