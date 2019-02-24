using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;

namespace SecteEcoPlus
{
    public static class RazorExtensions
    {
        public static Func<IHtmlContent> ToParameterLess(this Template template) => () => template(null);
    }

    public delegate IHtmlContent Template(object item);
}
