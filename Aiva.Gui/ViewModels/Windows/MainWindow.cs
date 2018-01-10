using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Aiva.Gui.ViewModels.Windows {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    internal class MainWindow {
        public Models.MainWindow Model { get; set; }
        public Models.MainWindow.TabItemsModel SelectedTab { get; set; }
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

            Model = new Models.MainWindow {
                WindowCommands = new Models.MainWindow.WindowCommandsModel {
                    RightWindowCommands = new List<Models.MainWindow.WindowCommandsModel.WindowCommandModel> {
                        new Models.MainWindow.WindowCommandsModel.WindowCommandModel {
                            Header = "Users",
                            Command = new Internal.RelayCommand(c => OpenUsers()),
                            Icon = Icons["users"] as Canvas
                        },
                        new Models.MainWindow.WindowCommandsModel.WindowCommandModel {
                            Header = "Event Window",
                            Command = new Internal.RelayCommand(e => OpenEventWindow()),
                            Icon = Icons["alert-box"] as Canvas
                        }
                    }
                },
                TabItems = new List<Models.MainWindow.TabItemsModel> {
                    new Models.MainWindow.TabItemsModel {
                        Header = "Console",
                        Content = new Views.Tabs.Console()
                    },
                    new Models.MainWindow.TabItemsModel {
                        Header = "Songrequest",
                        Content = new Views.Tabs.Songrequest(),
                        Flyouts = new List<MahApps.Metro.Controls.Flyout> {
                            new Views.Flyouts.HonorSongrequester()
                        }
                    },
                    new Models.MainWindow.TabItemsModel {
                        Header = "Giveaway",
                        Content = new Views.Tabs.Giveaway(),
                    },
                    new Models.MainWindow.TabItemsModel {
                        Header = "Timers",
                        Content = new Views.Tabs.Timers()
                    },
                    new Models.MainWindow.TabItemsModel {
                        Header = "Voting",
                        Content = new Views.Tabs.Voting()
                    },
                    new Models.MainWindow.TabItemsModel {
                        Header = "Streamgames",
                        Content = new Views.Tabs.Streamgames(),
                    },
                    new Models.MainWindow.TabItemsModel {
                        Header = "Spam Protection",
                        Content = new Views.Tabs.SpamProtection()
                    },
                }
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