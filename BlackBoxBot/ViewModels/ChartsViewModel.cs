using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using LiveCharts.Configurations;

namespace BlackBoxBot.ViewModels
{
    [PropertyChanged.ImplementPropertyChanged]
    class ChartsViewModel
    {
        public SeriesCollection LastHourSeries { get; set; }

        public ChartsViewModel()
        {
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
    class ViewerCount
    {
        public DateTime CurrentTime { get; set; }
        public int Viewercount { get; set; }
    }
}
