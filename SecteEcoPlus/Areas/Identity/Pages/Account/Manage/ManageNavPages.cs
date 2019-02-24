using System;
using System.Dynamic;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SecteEcoPlus.Areas.Identity.Pages.Account.Manage
{
    public static class ManageNavPages
    {
        public static string Index => "Index";

        public static string ChangePassword => "ChangePassword";

        public static string ExternalLogins => "ExternalLogins";

        public static string PersonalData => "PersonalData";

        public static string TwoFactorAuthentication => "TwoFactorAuthentication";

        public static string PublicProfile => nameof(PublicProfile);

        internal static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewBag.ActivePage as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }

    public class NavPagesClassManager : DynamicObject
    {
        public NavPagesClassManager(ViewContext context)
        {
            _context = context;
        }
        private readonly ViewContext _context;
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = ManageNavPages.PageNavClass(_context, binder.Name);
            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            if (binder.Name.EndsWith("NavClass"))
            {
                var name = binder.Name.Substring(0, binder.Name.Length - "NavClass".Length);
                result = ManageNavPages.PageNavClass(_context, name);
                return true;
            }

            result = null;
            return false;
        }
    }
}