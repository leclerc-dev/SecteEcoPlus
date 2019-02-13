using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SecteEcoPlus.Areas.Identity.Data;

namespace SecteEcoPlus.Areas.Identity
{
    public class SecteUserManager : UserManager<SecteUser>
    {
        protected SecteUserStore SecteUserStore { get; set; }
        public SecteUserManager(SecteUserStore store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<SecteUser> passwordHasher, IEnumerable<IUserValidator<SecteUser>> userValidators, IEnumerable<IPasswordValidator<SecteUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<SecteUser>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger) // gosh
        {
            SecteUserStore = store;
        }
        public async Task<string> GetDisplayNameAsync(SecteUser user)
        {
            var name = await SecteUserStore.GetDisplayNameByUserId(user.Id);
            return name;
        }

        public async Task<string> GetDisplayNameFromClaimAsync(ClaimsPrincipal c)
        {
            var id = GetUserId(c);
            if (id is null) return null;
            return await SecteUserStore.GetDisplayNameByUserId(id);
        }
        public async Task<SecteUser> GetUserWithProfileAsync(ClaimsPrincipal c)
        {
            var user = await GetUserAsync(c);
            user.PublicProfile = await GetPublicProfileByUserId(user.Id);
            return user;
        }

        public async Task<SecteUser> GetUserWithFullProfileAsync(ClaimsPrincipal c)
        {
            var user = await GetUserAsync(c);
            user.PublicProfile = await SecteUserStore.GetFullPublicProfileByUserId(user.Id);
            return user;
        }
        public async Task<SecteUser> GetUserWithProfileByIdAsync(string id)
        {
            var user = await FindByIdAsync(id);
            user.PublicProfile = await GetPublicProfileByUserId(id);
            return user;
        }
        public async Task<PublicProfile> GetPublicProfileByUserAsync(ClaimsPrincipal c)
        {
            var id = GetUserId(c);
            if (id is null) return null;
            return await GetPublicProfileByUserId(id);
        }
        public async Task<PublicProfile> GetPublicProfileByUserId(string id)
        {
            return await SecteUserStore.GetPublicProfileByUserId(id);
        }
    }
}