using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Aiva.Extensions.Models;

namespace Aiva.Extensions.Songrequest {

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Player {

        public Models.Songrequest.SongModel CurrentSong { get; set; }

        public void ChangeSong(Models.Songrequest.SongModel song) {
            CurrentSong = song;
        }
    }
}
