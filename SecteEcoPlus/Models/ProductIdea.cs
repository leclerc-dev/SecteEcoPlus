using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SecteEcoPlus.Areas.Identity.Data;

namespace SecteEcoPlus.Models
{
    public class ProductIdea
    {
        public int Id { get; set; }

        public PublicProfile Author { get; set; }
        public int AuthorPublicProfileId { get; set; }

        [StringLength(500, MinimumLength = 3)]
        public string Content { get; set; }

        [InverseProperty(nameof(ProductIdeaVote.ProductIdea))]
        public ICollection<ProductIdeaVote> Votes { get; set; }

        public DateTime PublishDate { get; set; } = DateTime.Now;
    }
}