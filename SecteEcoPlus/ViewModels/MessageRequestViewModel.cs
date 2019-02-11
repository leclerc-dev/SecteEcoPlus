using System.ComponentModel.DataAnnotations;

namespace SecteEcoPlus.ViewModels
{
    public class MessageRequestViewModel
    {
        [Display(Name = "Contenu")]
        [MaxLength(420)]
        [Required]
        public string Content { get; set; }

        [Display(Name = "Être anonyme")]
        public bool IsAnonymous { get; set; } = false;
    }
}