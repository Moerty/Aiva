using System.ComponentModel;

namespace Aiva.Extensions.Enums {
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