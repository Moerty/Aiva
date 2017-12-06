using System.ComponentModel;

namespace Aiva.Models.Enums {
    [TypeConverter(typeof(Extensions.EnumDescriptionTypeConverter))]
    public enum JoinPermission {
        [Description(nameof(Everyone))]
        Everyone,

        [Description(nameof(Subscriber))]
        Subscriber,

        [Description("Mod")]
        Moderation,
    }
}