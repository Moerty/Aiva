//------------------------------------------------------------------------------
// <auto-generated>
//     Der Code wurde von einer Vorlage generiert.
//
//     Manuelle Änderungen an dieser Datei führen möglicherweise zu unerwartetem Verhalten der Anwendung.
//     Manuelle Änderungen an dieser Datei werden überschrieben, wenn der Code neu generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Aiva.Core.Storage
{
    using System;
    using System.Collections.Generic;
    
    public partial class ActiveUsers
    {
        public long ID { get; set; }
        public System.DateTime JoinedTime { get; set; }
    
        public virtual Users Users { get; set; }
    }
}
