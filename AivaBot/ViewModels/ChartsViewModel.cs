using System;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace AivaBot.ViewModels {
    [PropertyChanged.ImplementPropertyChanged]
    class ChartsViewModel {
        public SeriesCollection LastHourSeries { get; set; }

        public ChartsViewModel() {
            LastHourSeries = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(3),
                        new ObservableValue(5)
                    }
                }
            };
        }
    }


    [PropertyChanged.ImplementPropertyChanged]
    class ViewerCount {
        public DateTime CurrentTime { get; set; }
        public int Viewercount { get; set; }
    }
}
