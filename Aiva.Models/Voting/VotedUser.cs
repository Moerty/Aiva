using Aiva.Models.Enums;

namespace Aiva.Models.Voting {
    public class VotedUser {
        public string Name { get; set; }
        public int Id { get; set; }
        public VotingOption Option { get; set; }
    }
}