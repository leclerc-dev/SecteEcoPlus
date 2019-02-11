using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SecteEcoPlus.Pages
{
    public class ErrorModel : PageModel
    {
        public string RequestId { get; set; }
        /// <summary>
        /// Whether or not the request id should be shown
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public void OnGet() { }
    }
}
