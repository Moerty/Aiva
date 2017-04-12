namespace Giveaway.Models {
    [PropertyChanged.ImplementPropertyChanged]
    public class UsersModel {
        public string Username { get; set; }
        public bool IsSub { get; set; }
    }
}
