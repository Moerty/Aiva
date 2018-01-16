using System;
using System.Collections.Generic;
using System.Linq;

namespace Aiva.Core.Database.Handlers {
    public class Statistics {
        /// <summary>
        /// Get statistics for the last days
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        public List<Storage.ViewerStatistics> GetLastDays(int days = 20) {
            using (var context = new Storage.DatabaseContext()) {
                var toDate = DateTime.Now.AddDays(-days);
                return context.ViewerStatistics.Where(d => d.Date.Date <= toDate.Date).ToList();
            }
        }

        /// <summary>
        /// Get todays statistic
        /// </summary>
        /// <returns></returns>
        public Storage.ViewerStatistics GetToday() {
            using (var context = new Storage.DatabaseContext()) {
                return context.ViewerStatistics.SingleOrDefault(d => d.Date.Date == DateTime.Now.Date);
            }
        }

        /// <summary>
        /// Add / edit all values from today statistic
        /// </summary>
        /// <param name="count"></param>
        public void AddViewerCountToDatabase(int count) {
            using (var context = new Storage.DatabaseContext()) {
                var today = context.ViewerStatistics
                    .SingleOrDefault(d => d.Date.Date == DateTime.Now.Date);

                // check if created
                if (today == null) {
                    CreateOrResetTodaysViewerCountStatistic();
                    AddViewerCountToDatabase(count);
                    return;
                } else {
                    if (count < today.Min) {
                        today.Min = count;
                    }

                    if (count > today.Max) {
                        today.Max = count;
                    }

                    today.Count++;
                    today.AddedValue += count;
                    today.Average = today.AddedValue / today.Count;

                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Create or reset todays statistic
        /// </summary>
        private void CreateOrResetTodaysViewerCountStatistic() {
            using (var context = new Storage.DatabaseContext()) {
                var today = context.ViewerStatistics
                    .SingleOrDefault(d => d.Date.Date == DateTime.Now.Date);

                if (today != null) {
                    today.Average = 0;
                    today.Max = 0;
                    today.Min = 0;
                    today.AddedValue = 0;
                    today.Count = 0;
                } else {
                    today = new Storage.ViewerStatistics {
                        Average = 0,
                        Max = 0,
                        Min = 0,
                        AddedValue = 0,
                        Count = 0,
                        Date = DateTime.Now
                    };
                    context.ViewerStatistics.Add(today);
                }

                context.SaveChanges();
            }
        }
    }
}