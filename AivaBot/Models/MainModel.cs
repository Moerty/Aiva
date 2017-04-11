using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AivaBot.Models {
    [PropertyChanged.ImplementPropertyChanged]
    public class MainModel {

        public Button SubMenu { get; set; }
        public string StreamerOnlineText { get; set; }
        public bool IsOnline { get; set; } = false;

        public MahApps.Metro.Controls.MetroContentControl Content { get; set; }
        public ObservableCollection<WindowCommandModel> WindowCommandItems { get; set; }

        [PropertyChanged.ImplementPropertyChanged]
        public class WindowCommandModel {
            public string Header { get; set; }
            public ICommand Command { get; set; } = new RoutedCommand();
            public Visual Icon { get; set; }
        }
    }

    public class AsyncObservableCollection<T> : ObservableCollection<T> {
        private SynchronizationContext _synchronizationContext = SynchronizationContext.Current;

        public AsyncObservableCollection() {
        }

        public AsyncObservableCollection(IEnumerable<T> list)
            : base(list) {
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e) {
            if (SynchronizationContext.Current == _synchronizationContext) {
                // Execute the CollectionChanged event on the current thread
                RaiseCollectionChanged(e);
            }
            else {
                // Raises the CollectionChanged event on the creator thread
                _synchronizationContext.Send(RaiseCollectionChanged, e);
            }
        }

        private void RaiseCollectionChanged(object param) {
            // We are in the creator thread, call the base implementation directly
            base.OnCollectionChanged((NotifyCollectionChangedEventArgs)param);
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e) {
            if (SynchronizationContext.Current == _synchronizationContext) {
                // Execute the PropertyChanged event on the current thread
                RaisePropertyChanged(e);
            }
            else {
                // Raises the PropertyChanged event on the creator thread
                _synchronizationContext.Send(RaisePropertyChanged, e);
            }
        }

        private void RaisePropertyChanged(object param) {
            // We are in the creator thread, call the base implementation directly
            base.OnPropertyChanged((PropertyChangedEventArgs)param);
        }
    }
}