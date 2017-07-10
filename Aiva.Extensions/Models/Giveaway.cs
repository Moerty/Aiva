using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Aiva.Extensions.Models {

    [TypeConverter(typeof(Extensions.EnumDescriptionTypeConverter))]
    public enum JoinPermission {
        [Description(nameof(Everyone))]
        Everyone,

        [Description("Subscriber")]
        Subscriber,

        [Description("Mod")]
        Moderation,
    }

    public class Giveaway {

    }
}
