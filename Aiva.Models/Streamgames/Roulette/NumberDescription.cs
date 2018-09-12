using System;
using System.Collections.Generic;
using System.Text;

namespace Aiva.Models.Streamgames.Roulette {
    public class NumberDescription {
        public int Number { get; set; }
        public bool IsZero { get; set; }
        public bool IsEven { get; set; }
        public bool IsRed { get; set; }
        public bool IsFirstHalf { get; set; }
        public int ColumnNo { get; set; }
        public int RowNo { get; set; }
    }
}
