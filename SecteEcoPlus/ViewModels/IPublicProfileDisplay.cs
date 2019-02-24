using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SecteEcoPlus.ViewModels
{
    public interface IPublicProfileDisplay
    {
        int Id { get; set; }
        [DisplayFormat(NullDisplayText = "Anonyme")]
        string DisplayName { get; set; }
    }
}