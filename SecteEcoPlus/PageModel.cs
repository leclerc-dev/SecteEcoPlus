using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;

namespace SecteEcoPlus
{
    public class PageModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
    {
        private DynamicViewData _viewBag;

        public dynamic ViewBag => _viewBag ?? (_viewBag = new DynamicViewData(() => ViewData));
    }
}
