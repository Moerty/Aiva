using Aiva.Bot.Internal;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            var myResourceDictionary = new ResourceDictionary {
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
                                Content = new Views.Flyouts.UserInfo(),
                            }
                        },
                        WindowCommands = new Models.MainWindow.TabItemsModel.WindowCommandsModel {
                            LeftWindowCommands = new System.Collections.ObjectModel.ObservableCollection<Models.MainWindow.TabItemsModel.WindowCommandsModel.WindowCommandModel> {
                                new Models.MainWindow.TabItemsModel.WindowCommandsModel.WindowCommandModel {
                                    Header = "Users",
                                    Command = new RelayCommand(c => OpenUsers(), c => true),
                                    Icon = myResourceDictionary["users"] as Canvas
                                }
                            }
                        }
                    },
                    new Models.MainWindow.TabItemsModel {
                        Header = "Test1",
                        Content = new MahApps.Metro.Controls.MetroContentControl(),
                    }
                }
            };


            Instance = this;
        }

        private void OpenUsers()
        {
            var users = new Views.Users();
            users.Show();
        }
    }
}
