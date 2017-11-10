namespace Aiva.Extensions.Songrequest {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Player {
        public Models.Songrequest.SongModel CurrentSong { get; set; }

        public void ChangeSong(Models.Songrequest.SongModel song) {
            CurrentSong = song;
        }
    }
}