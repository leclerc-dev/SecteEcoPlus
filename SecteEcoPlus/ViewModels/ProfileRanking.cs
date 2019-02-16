using SecteEcoPlus.Experience;

namespace SecteEcoPlus.ViewModels
{
    public class ProfileRanking : ExperienceObject
    {
        public int Id { get; set; }
        public int Rank { get; set; }
        public string DisplayName { get; set; }
    }
}