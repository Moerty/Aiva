namespace Aiva.Gui.Internal {
    using System.Windows;
    using System.Windows.Controls;

    //  Helpers to add autoscrolling functionality to ScrollViewer control
    public static class ScrollViewerExtension {
        public static bool GetAutoScroll(DependencyObject obj) {
            return (bool)obj.GetValue(AutoScrollProperty);
        }

        public static void SetAutoScroll(DependencyObject obj, bool value) {
            obj.SetValue(AutoScrollProperty, value);
        }

        public static readonly DependencyProperty AutoScrollProperty =
            DependencyProperty.RegisterAttached("AutoScroll", typeof(bool), typeof(ScrollViewerExtension), new PropertyMetadata(false, AutoScrollPropertyChanged));

        private static void AutoScrollPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var scrollViewer = d as ScrollViewer;

            if (scrollViewer != null && (bool)e.NewValue) {
                scrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;
            } else {
                scrollViewer.ScrollChanged -= ScrollViewer_ScrollChanged;
            }
        }

        private static void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e) {
            var scrollViewer = sender as ScrollViewer;
            if (e.ExtentHeightChange > 0)
                scrollViewer.ScrollToBottom();
        }
    }
}