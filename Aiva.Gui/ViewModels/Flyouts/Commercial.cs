﻿using PropertyChanged;
using System;
using System.Windows.Input;

namespace Aiva.Gui.ViewModels.Flyouts {
    [AddINotifyPropertyChangedInterface]
    public class Commercial {
        public Aiva.Models.Enums.Commercial CommercialLength { get; set; }

        public ICommand ShowCommercialCommand { get; set; }

        public EventHandler<TwitchLib.Enums.CommercialLength> OnClose;

        public Commercial() {
            CommercialLength = Aiva.Models.Enums.Commercial.Seconds90;
            ShowCommercialCommand = new Internal.RelayCommand(
                show => ShowCommercial(),
                show => CommercialLength.HasFlag(CommercialLength));
        }

        private void ShowCommercial() {
            var selectedCommercialLength = GetCommercialLengthTwitchlib();

            OnClose?.Invoke(this, selectedCommercialLength);
        }

        private TwitchLib.Enums.CommercialLength GetCommercialLengthTwitchlib() {
            switch (CommercialLength) {
                case Aiva.Models.Enums.Commercial.Seconds120:
                    return TwitchLib.Enums.CommercialLength.Seconds120;
                case Aiva.Models.Enums.Commercial.Seconds150:
                    return TwitchLib.Enums.CommercialLength.Seconds150;
                case Aiva.Models.Enums.Commercial.Seconds180:
                    return TwitchLib.Enums.CommercialLength.Seconds180;
                case Aiva.Models.Enums.Commercial.Seconds30:
                    return TwitchLib.Enums.CommercialLength.Seconds30;
                case Aiva.Models.Enums.Commercial.Seconds60:
                    return TwitchLib.Enums.CommercialLength.Seconds60;
                default:
                    return TwitchLib.Enums.CommercialLength.Seconds90;
            }
        }
    }
}