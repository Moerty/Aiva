using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
