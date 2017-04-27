using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiva.Bot.ViewModels {
    [PropertyChanged.ImplementPropertyChanged]
    class MainWindow {
        public Models.MainWindow Model { get; set; }
        public Models.MainWindow.TabItemsModel SelectedTab { get; set; }

        public MainWindow() {

            Model = new Models.MainWindow {
                TabItems = new System.Collections.ObjectModel.ObservableCollection<Models.MainWindow.TabItemsModel> {
                    new Models.MainWindow.TabItemsModel {
                        Header = "Console",
                        Content = new Views.Console(),
                        Flyouts = new List<Models.FlyoutItem> {
                            new Models.FlyoutItem {
                                Header = "TEST",
                                IsOpen = false,
                                Position = MahApps.Metro.Controls.Position.Right
                            }
                        }
                    },
                    new Models.MainWindow.TabItemsModel {
                        Header = "Test1",
                        Content = new MahApps.Metro.Controls.MetroContentControl(),
                    }
                }
            };
        }
    }
}
