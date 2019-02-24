using System.ComponentModel.DataAnnotations;

namespace SecteEcoPlus.ViewModels
{
    public class PublicProfileViewModel : IPublicProfileDisplay
    {
        public int Id { get; set; }
        [Display(Name = "Pseudonyme", ShortName = "Pseudo")]
        [DisplayFormat(NullDisplayText = "Anonyme")]
        public string DisplayName { get; set; }
    }
}