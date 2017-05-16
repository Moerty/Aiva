using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Aiva.Bot.Internal {

    public class CollectionViewSourceWithLiveSorting : CollectionViewSource {
        #region Overridden Methods

        protected override void OnSourceChanged(object oldSource, object newSource) {
            base.OnSourceChanged(oldSource, newSource);

            UnHookCollectionChangedFromSource(oldSource);
            HookUpCollectionChanged();
        }

        #endregion OverriddenMethods

        #region Private Methods

        private void UnHookCollectionChangedFromSource(object oldSource) {
            if (oldSource != null) {
                var oldSourceCollection = oldSource as INotifyCollectionChanged;
                if (oldSourceCollection != null) {
                    oldSourceCollection.CollectionChanged -= SourceCollection_CollectionChanged;
                }
            }
        }

        private void HookUpCollectionChanged() {
            var sourceCollection = this.Source as INotifyCollectionChanged;
            if (sourceCollection != null) {
                sourceCollection.CollectionChanged += SourceCollection_CollectionChanged;
            }

            var sourceEnumerable = this.Source as IEnumerable<INotifyPropertyChanged>;
            if (sourceEnumerable != null) {
                foreach (var notifyPropertyChanged in sourceEnumerable) {
                    notifyPropertyChanged.PropertyChanged += NotifyPropertyChanged_PropertyChanged;
                }
            }
        }

        private void NotifyPropertyChanged_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (this.SortDescriptions != null && this.SortDescriptions.Any() &&
                this.SortDescriptions.Any(s => string.Equals(s.PropertyName, e.PropertyName))) {
                Application.Current.Dispatcher.Invoke(() => this.View.Refresh());
            }

        }

        private void SourceCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            switch (e.Action) {
                case NotifyCollectionChangedAction.Remove: {
                        foreach (var oldItem in e.OldItems) {
                            var oldItemNotifyPropertyChanged = oldItem as INotifyPropertyChanged;
                            if (oldItemNotifyPropertyChanged != null) {
                                oldItemNotifyPropertyChanged.PropertyChanged -= NotifyPropertyChanged_PropertyChanged;
                            }
                        }
                    }
                    break;

                case NotifyCollectionChangedAction.Add: {
                        foreach (var newItem in e.NewItems) {
                            var newItemNotifyPropertyChanged = newItem as INotifyPropertyChanged;
                            if (newItemNotifyPropertyChanged != null) {
                                newItemNotifyPropertyChanged.PropertyChanged += NotifyPropertyChanged_PropertyChanged;
                            }
                        }
                    }
                    break;
            }
        }

        #endregion Private Methods
    }
}