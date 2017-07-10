using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiva.Bot.ViewModels {

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Giveaway {
        List<string> JoinPermissions = new List<string>();

        public Giveaway() {
            //SetModels();
        }

        //private void SetModels() {
        //    var joinPermissions = Enum.GetValues(typeof(Extensions.Models.Giveaway.JoinPermission))
        //        .Cast<Enum>()
        //        .Select(value => new {
        //            (Attribute.GetCustomAttribute(value.GetType().GetField(value.ToString()), typeof(DescriptionAttribute)) as DescriptionAttribute).Description
        //        })
        //        .OrderBy(item => item.Description)
        //        .ToList();

        //    joinPermissions.ForEach(value => JoinPermissions.Add(value.Description));
        //}
    }
}
