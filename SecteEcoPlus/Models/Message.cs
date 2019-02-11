using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SecteEcoPlus.Areas.Identity.Data;

namespace SecteEcoPlus.Models
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageId { get; set; }

        public int? AuthorId { get; set; }
        [Display(Name = "Auteur")]
        [ForeignKey("AuthorId")]
        public PublicProfile Author { get; set; }

        [Display(Name = "Contenu")]
        [MaxLength(420)]
        [Required]
        public string Content { get; set; }
        [Display(Name = "Date de publication")]
        [DisplayFormat(DataFormatString = "{0:D} à {0:t}")]
        public DateTime PublishDate { get; set; }
    }
}   