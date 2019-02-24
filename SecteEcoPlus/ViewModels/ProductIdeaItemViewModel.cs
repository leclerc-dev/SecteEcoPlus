using System;
using System.ComponentModel.DataAnnotations;

namespace SecteEcoPlus.ViewModels
{
    public class ProductIdeaItemViewModel
    {
        public int Id { get; set; }
        public PublicProfileViewModel Author { get; set; }

        public string Content { get; set; }
        public int TotalVotes { get; set; }
        [DisplayFormat(DataFormatString = "{0:g}", NullDisplayText = "[inconnu]")]
        public DateTime PublishDate { get; set; }

        public int VoteDirection { get; set; }
        public bool IsFromUser { get; set; }
    }
}