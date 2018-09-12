using System;
using System.Collections.Generic;
using System.Text;

namespace Aiva.Models.Streamgames.Roulette {
    public class TableLayout {
        public List<NumberDescription> AllNumbers { get; set; }

        public TableLayout() {
            AllNumbers = new List<NumberDescription> {
                new NumberDescription() { Number = 0, IsZero = true },
                new NumberDescription() { Number = 0, IsZero = true },

                new NumberDescription() { Number = 1, IsZero = false, IsEven = false, IsRed = true, IsFirstHalf = true, ColumnNo = 1, RowNo = 3 },
                new NumberDescription() { Number = 2, IsZero = false, IsEven = true, IsRed = false, IsFirstHalf = true, ColumnNo = 1, RowNo = 2 },
                new NumberDescription() { Number = 3, IsZero = false, IsEven = false, IsRed = true, IsFirstHalf = true, ColumnNo = 1, RowNo = 1 },
                new NumberDescription() { Number = 4, IsZero = false, IsEven = true, IsRed = false, IsFirstHalf = true, ColumnNo = 1, RowNo = 3 },
                new NumberDescription() { Number = 5, IsZero = false, IsEven = false, IsRed = true, IsFirstHalf = true, ColumnNo = 1, RowNo = 2 },
                new NumberDescription() { Number = 6, IsZero = false, IsEven = true, IsRed = false, IsFirstHalf = true, ColumnNo = 1, RowNo = 1 },
                new NumberDescription() { Number = 7, IsZero = false, IsEven = false, IsRed = true, IsFirstHalf = true, ColumnNo = 1, RowNo = 3 },
                new NumberDescription() { Number = 8, IsZero = false, IsEven = true, IsRed = false, IsFirstHalf = true, ColumnNo = 1, RowNo = 2 },
                new NumberDescription() { Number = 9, IsZero = false, IsEven = false, IsRed = true, IsFirstHalf = true, ColumnNo = 1, RowNo = 1 },
                new NumberDescription() { Number = 10, IsZero = false, IsEven = true, IsRed = false, IsFirstHalf = true, ColumnNo = 1, RowNo = 3 },
                new NumberDescription() { Number = 11, IsZero = false, IsEven = false, IsRed = false, IsFirstHalf = true, ColumnNo = 1, RowNo = 2 },
                new NumberDescription() { Number = 12, IsZero = false, IsEven = true, IsRed = true, IsFirstHalf = true, ColumnNo = 1, RowNo = 1 },
                new NumberDescription() { Number = 13, IsZero = false, IsEven = false, IsRed = false, IsFirstHalf = true, ColumnNo = 2, RowNo = 3 },
                new NumberDescription() { Number = 14, IsZero = false, IsEven = true, IsRed = true, IsFirstHalf = true, ColumnNo = 2, RowNo = 2 },
                new NumberDescription() { Number = 15, IsZero = false, IsEven = false, IsRed = false, IsFirstHalf = true, ColumnNo = 2, RowNo = 1 },
                new NumberDescription() { Number = 16, IsZero = false, IsEven = true, IsRed = true, IsFirstHalf = true, ColumnNo = 2, RowNo = 3 },
                new NumberDescription() { Number = 17, IsZero = false, IsEven = false, IsRed = false, IsFirstHalf = true, ColumnNo = 2, RowNo = 2 },
                new NumberDescription() { Number = 18, IsZero = false, IsEven = true, IsRed = true, IsFirstHalf = true, ColumnNo = 2, RowNo = 1 },
                new NumberDescription() { Number = 19, IsZero = false, IsEven = false, IsRed = true, IsFirstHalf = false, ColumnNo = 2, RowNo = 3 },
                new NumberDescription() { Number = 20, IsZero = false, IsEven = true, IsRed = false, IsFirstHalf = false, ColumnNo = 2, RowNo = 2 },
                new NumberDescription() { Number = 21, IsZero = false, IsEven = false, IsRed = true, IsFirstHalf = false, ColumnNo = 2, RowNo = 1 },
                new NumberDescription() { Number = 22, IsZero = false, IsEven = true, IsRed = false, IsFirstHalf = false, ColumnNo = 2, RowNo = 3 },
                new NumberDescription() { Number = 23, IsZero = false, IsEven = false, IsRed = true, IsFirstHalf = false, ColumnNo = 2, RowNo = 2 },
                new NumberDescription() { Number = 24, IsZero = false, IsEven = true, IsRed = false, IsFirstHalf = false, ColumnNo = 2, RowNo = 1 },
                new NumberDescription() { Number = 25, IsZero = false, IsEven = false, IsRed = true, IsFirstHalf = false, ColumnNo = 3, RowNo = 3 },
                new NumberDescription() { Number = 26, IsZero = false, IsEven = true, IsRed = false, IsFirstHalf = false, ColumnNo = 3, RowNo = 2 },
                new NumberDescription() { Number = 27, IsZero = false, IsEven = false, IsRed = true, IsFirstHalf = false, ColumnNo = 3, RowNo = 1 },
                new NumberDescription() { Number = 28, IsZero = false, IsEven = true, IsRed = false, IsFirstHalf = false, ColumnNo = 3, RowNo = 3 },
                new NumberDescription() { Number = 29, IsZero = false, IsEven = false, IsRed = false, IsFirstHalf = false, ColumnNo = 3, RowNo = 2 },
                new NumberDescription() { Number = 30, IsZero = false, IsEven = true, IsRed = true, IsFirstHalf = false, ColumnNo = 3, RowNo = 1 },
                new NumberDescription() { Number = 31, IsZero = false, IsEven = false, IsRed = false, IsFirstHalf = false, ColumnNo = 3, RowNo = 3 },
                new NumberDescription() { Number = 32, IsZero = false, IsEven = true, IsRed = true, IsFirstHalf = false, ColumnNo = 3, RowNo = 2 },
                new NumberDescription() { Number = 33, IsZero = false, IsEven = false, IsRed = false, IsFirstHalf = false, ColumnNo = 3, RowNo = 1 },
                new NumberDescription() { Number = 34, IsZero = false, IsEven = true, IsRed = true, IsFirstHalf = false, ColumnNo = 3, RowNo = 3 },
                new NumberDescription() { Number = 35, IsZero = false, IsEven = false, IsRed = false, IsFirstHalf = false, ColumnNo = 3, RowNo = 2 },
                new NumberDescription() { Number = 36, IsZero = false, IsEven = true, IsRed = true, IsFirstHalf = false, ColumnNo = 3, RowNo = 1 }
            };
        }
    }
}
