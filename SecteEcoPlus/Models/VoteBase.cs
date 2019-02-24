using SecteEcoPlus.Areas.Identity.Data;

namespace SecteEcoPlus.Models
{
    public class VoteBase
    {
        public int Id { get; set; }
        public PublicProfile Issuer { get; set; }
        public int? IssuerPublicProfileId { get; set; }
        public int Direction { get; set; }
    }
}