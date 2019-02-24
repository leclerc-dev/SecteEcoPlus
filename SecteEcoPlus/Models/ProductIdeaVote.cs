using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SecteEcoPlus.Areas.Identity.Data;

namespace SecteEcoPlus.Models
{
    public class ProductIdeaVote : VoteBase
    {
        public ProductIdea ProductIdea { get; set; }
        public int ProductIdeaId { get; set; }
    }
}