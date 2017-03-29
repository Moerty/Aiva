using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BlackBoxBot.Models
{
    public class BetsModel
    {
        public TextModel Text { get; set; }
        public List<DropDownButtonModel> DropDownMenu { get; set; } = new List<DropDownButtonModel>();

        public class TextModel
        {
            public string CommandWatermark { get; set; }
            public string TextBoxTextTimeForBet { get; set; }
            public string StartStopButtonText { get; set; }
            public string PayOutButtonText { get; set; }
            public string ExpanderUsersName { get; set; }
            public string BetOption1Text { get; set; } //= ;
            public string CountName { get; set; } //= "Anzahl der Wetter";
            public string BetOption2Text { get; set; } //= "Wette 2";
            public string ValueName { get; set; } //= "Wetteinsatz";
            public string GridTeamHeaderName { get; set; }
            public string GridBetValueHeaderName { get; set; }
            public string GridBetterHeaderName { get; set; }
        }

        public class DropDownButtonModel
        {
            public string Name { get; set; }
            public ICommand Command { get; set; } = new RoutedCommand();
        }
    }
}
