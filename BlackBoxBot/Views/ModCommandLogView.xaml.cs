using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Database;

namespace BlackBoxBot.Views
{
	/// <summary>
	/// Interaktionslogik für ModCommandLog.xaml
	/// </summary>
	public partial class ModCommandLogViewer : MahApps.Metro.Controls.MetroWindow
	{
		public ObservableCollection<LogEntry> LogEntries { get; set; }

		public ModCommandLogViewer()
		{
			InitializeComponent();

			List<ModCommandLog> log;
			using (var context = new DatabaseEntities())
			{
				log = context.ModCommandLog.ToList();
			}
			DataContext = LogEntries = new ObservableCollection<LogEntry>();

			foreach(var entry in log)
			{
				LogEntries.Add(CreateLogEntry(entry));
			}
		}

        private static LogEntry CreateLogEntry(ModCommandLog entry) => new LogEntry
        {
            DateTime = DateTime.Parse(entry.Timestamp),
            Name = entry.Name,
            Message = entry.Command
        };
    }

	public class LogEntry : PropertyChangedBase
	{
		public DateTime DateTime { get; set; }

		public string Name { get; set; }

		public string Message { get; set; }
	}

	class CollapsibleLogEntry : LogEntry
	{
		public List<LogEntry> Contents { get; set; }
	}

	public class PropertyChangedBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			Application.Current.Dispatcher.BeginInvoke((Action)(() =>
			{
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}));
		}
	}
}
