using Aiva.Bot.Internal;
using MahApps.Metro.Controls;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Aiva.Bot.ViewModels {
    [PropertyChanged.ImplementPropertyChanged]
    class MainWindow {

        private static MainWindow _Instance;
        public static MainWindow Instance {
            get {
                return _Instance;
            }
            private set {
                _Instance = value;
            }
        }

        public Models.MainWindow Model { get; set; }
        public Models.MainWindow.TabItemsModel SelectedTab { get; set; }

        public MainWindow() {


            InitViewModel();

            Instance = this;
        }

        private void InitViewModel() {
            var Icons = new ResourceDictionary {
                Source =
                new Uri("/AivaBot;component/Styles/Icons.xaml",
                        UriKind.RelativeOrAbsolute)
            };

            Model = new Models.MainWindow {
                TabItems = new System.Collections.ObjectModel.ObservableCollection<Models.MainWindow.TabItemsModel> {
                    new Models.MainWindow.TabItemsModel {
                        Header = "Console",
                        Content = new Views.Console(),
                        Flyouts = new System.Collections.ObjectModel.ObservableCollection<Flyout> {
                            new Flyout {
                                Header = "UserDetails",
                                Content = new Views.Flyouts.UserInfo(null),
                            }
                        },
                        WindowCommands = new Models.MainWindow.TabItemsModel.WindowCommandsModel {
                            RightWindowCommands = new System.Collections.ObjectModel.ObservableCollection<Models.MainWindow.TabItemsModel.WindowCommandsModel.WindowCommandModel> {
                                new Models.MainWindow.TabItemsModel.WindowCommandsModel.WindowCommandModel {
                                    Header = "Users",
                                    Command = new RelayCommand(c => OpenUsers(), c => true),
                                    Icon = Icons["users"] as Canvas
                                }
                            }
                        }
                    },
                    new Models.MainWindow.TabItemsModel {
                        Header = "Songrequest",
                        Content = new Views.Songrequest(),
                        Flyouts = new System.Collections.ObjectModel.ObservableCollection<Flyout> {
                            new Flyout {
                                Header = "Honor Requester",
                                Content = new Views.Flyouts.HonorSongrequester(),
                                Position = Position.Right
                            }
                        }
                    },
                    new Models.MainWindow.TabItemsModel {
                        Header = "Commands",
                        Content = new Views.Commands()
                    },
                    new Models.MainWindow.TabItemsModel {
                        Header = "Timers",
                        Content = new Views.Timers(),
                    }
                }
            };
        }

        private void OpenUsers() {
            var users = new Views.Users();
            users.Show();
        }
    }
}
