using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SecteEcoPlus.Areas.Identity.Data;
using SecteEcoPlus.Models;

namespace SecteEcoPlus.Areas.Identity
{
    public class SecteUserStore : UserStore<SecteUser>
    {
        private WebsiteContext _context;
        public SecteUserStore(WebsiteContext context, IdentityErrorDescriber describer = null) : base(context, describer)
        {
            _context = context;
        }

        public async Task<string> GetDisplayNameByUserId(string id)
        {
            return (await GetPublicProfileByUserId(id))?.DisplayName;
        }
        public async Task<PublicProfile> GetPublicProfileByUserId(string id)
        {
            Debug.WriteLine(nameof(GetPublicProfileByUserId));
            var user = await FindByIdAsync(id);
            if (user is null) return null;
            return await GetPublicProfileById(user.PublicProfileId);
        }
        public async Task<PublicProfile> GetPublicProfileById(int id)
        {
            return await _context.PublicProfiles.FindAsync(id);
        }
        public async Task<T> GetPublicProfileByUserId<T>(string id, Expression<Func<PublicProfile, T>> selectExpression)
        {
            var profileCol = Users
                .Where(u => u.Id.Equals(id))
                .Select(u => u.PublicProfile)
                .Select(selectExpression);
            return await profileCol.FirstOrDefaultAsync();
        }
        public async Task<PublicProfile> GetFullPublicProfileByUserId(string id)
        {
            var profileCol = Users
                .Where(u => u.Id.Equals(id))
                .Select(u => u.PublicProfile)
                .Include(p => p.ReviewMessages);
            return await profileCol.FirstOrDefaultAsync();
        }

        public override async Task<IdentityResult> DeleteAsync(SecteUser user, CancellationToken cancellationToken = new CancellationToken())
        {
            var a = await base.DeleteAsync(user, cancellationToken);
            _context.PublicProfiles.Remove(new PublicProfile { PublicProfileId = user.PublicProfileId });
            await _context.SaveChangesAsync(cancellationToken);
            return a;
        }
    }
}
