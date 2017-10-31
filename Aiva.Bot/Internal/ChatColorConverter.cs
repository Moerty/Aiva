using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using Drawing = System.Drawing;

namespace Aiva.Bot.Internal {
    public class ChatColorConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {

            var converted = (Drawing.Color)value;
            var color = Drawing.Color.FromArgb(converted.A, converted.R, converted.G, converted.B);
            return new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            if (targetType == typeof(Drawing.Color)) {

                SolidColorBrush converted;
                if ((converted = value as SolidColorBrush) != null) {
                    var color = Drawing.Color.FromArgb(converted.Color.A, converted.Color.R, converted.Color.G, converted.Color.B);

                    return color;
                }
            }

            return null;
        }
    }
}
