using System.ComponentModel;

namespace Aiva.Models.Enums {
    [TypeConverter(typeof(Extensions.EnumDescriptionTypeConverter))]
    public enum Commercial {
        [Description("30 seconds")]
        Seconds30 = 30,

        [Description("60 seconds")]
        Seconds60 = 60,

        [Description("90 seconds")]
        Seconds90 = 90,

        [Description("120 seconds")]
        Seconds120 = 120,

        [Description("150 seconds")]
        Seconds150 = 150,

        [Description("180 seconds")]
        Seconds180 = 180
    }
}