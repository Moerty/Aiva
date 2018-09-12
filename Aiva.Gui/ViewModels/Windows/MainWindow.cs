using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Aiva.Gui.ViewModels.Windows {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    internal class MainWindow {
        public Models.MainWindow Model { get; set; }
        public Views.Windows.Events Events { get; set; }

        public MainWindow() {
            InitViewModel();
        }

        private void InitViewModel() {
            var Icons = new ResourceDictionary {
                Source =
                new Uri("/Aiva.Gui;component/Styles/Icons.xaml",
                        UriKind.RelativeOrAbsolute)
            };

        }

        private void OpenUsers() {
            var users = new Views.Windows.Users();
            users.Show();
        }

        private void OpenEventWindow() {
            var window = new Views.Windows.Events();
            window.Show();
        }
    }
}