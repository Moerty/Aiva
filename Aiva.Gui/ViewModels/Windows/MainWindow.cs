using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Aiva.Gui.ViewModels.Windows {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    internal class MainWindow {
        public Models.MainWindow Model { get; set; }
        public Models.MainWindow.TabItemsModel SelectedTab { get; set; }

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
                TabItems = new List<Models.MainWindow.TabItemsModel> {
                    new Models.MainWindow.TabItemsModel {
                        Header = "Console",
                        Content = new Views.Tabs.Console(),
                        WindowCommands = new Models.MainWindow.TabItemsModel.WindowCommandsModel {
                            RightWindowCommands = new List<Models.MainWindow.TabItemsModel.WindowCommandsModel.WindowCommandModel> {
                                new Models.MainWindow.TabItemsModel.WindowCommandsModel.WindowCommandModel {
                                    Header = "Users",
                                    Command = new Internal.RelayCommand(c => OpenUsers(), c => true),
                                    Icon = Icons["users"] as Canvas
                                }
                            }
                        }
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

                        //Flyouts = new System.Collections.ObjectModel.ObservableCollection<Flyout> {
                        //    new Flyout {
                        //        Header = "UserDetails",
                        //        Content = new Views.Flyouts.UserInfo(null),
                        //    }
                        //},
                //        WindowCommands = new Models.MainWindow.TabItemsModel.WindowCommandsModel {
                //            RightWindowCommands = new System.Collections.ObjectModel.ObservableCollection<Models.MainWindow.TabItemsModel.WindowCommandsModel.WindowCommandModel> {
                //                new Models.MainWindow.TabItemsModel.WindowCommandsModel.WindowCommandModel {
                //                    Header = "Users",
                //                    Command = new RelayCommand(c => OpenUsers(), c => true),
                //                    Icon = Icons["users"] as Canvas
                //                }
                //            }
                //        }
                //    },
                //    new Models.MainWindow.TabItemsModel {
                //        Header = "Songrequest",
                //        Content = new Views.Songrequest()
                //    },
                //    //new Models.MainWindow.TabItemsModel {
                //    //    Header = "Commands",
                //    //    Content = new Views.Commands()
                //    //},
                //    new Models.MainWindow.TabItemsModel {
                //        Header = "Giveaway",
                //        Content = new Views.Giveaway()
                //    },

                //    new Models.MainWindow.TabItemsModel {
                //        Header = "Voting",
                //        Content = new Views.Voting()
                //    },

                //}
            }
            };
        }

        private void OpenUsers() {
            var users = new Views.Windows.Users();
            users.Show();
        }
    }
}