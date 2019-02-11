using System;
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
        public SecteUserStore(WebsiteContext context, IdentityErrorDescriber describer = null) : base(context, describer)
        {
        }

        public async Task<int> GetPublicProfileIdByUserId(string id)
        {
            return (await Users
                .Select(u => new
                {
                    u.PublicProfile.SecteUserId,
                    u.PublicProfile.PublicProfileId
                })
                .FirstOrDefaultAsync(n => n.SecteUserId == id)).PublicProfileId;
        }
        public async Task<PublicProfile> GetPublicProfileByUserId(string id)
        {
            var profileCol = Users
                .Where(u => u.Id.Equals(id))
                .Select(u => u.PublicProfile);
            return await profileCol.FirstOrDefaultAsync();
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
    }
}
