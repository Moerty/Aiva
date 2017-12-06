using Aiva.Core.Database.Storage;
using System.Collections.Generic;
using System.Linq;

namespace Aiva.Core.Database.Handlers {
    public class SpamProtection {
        public BlacklistHandler Blacklist;

        public SpamProtection() {
            Blacklist = new BlacklistHandler();
        }

        public class BlacklistHandler {
            public List<BlacklistedWords> GetBlacklistedWords() {
                using (var context = new DatabaseContext()) {
                    return context.BlacklistedWords.ToList();
                }
            }

            public void AddWord(string wordToAdd) {
                using (var context = new DatabaseContext()) {
                    var word = new BlacklistedWords { Word = wordToAdd };
                    context.BlacklistedWords.Add(word);
                    context.SaveChanges();
                }
            }

            public void RemoveWord(string word) {
                using (var context = new DatabaseContext()) {
                    var entry = context.BlacklistedWords.SingleOrDefault(w => string.Compare(w.Word, word) == 0);

                    if (entry != null) {
                        context.BlacklistedWords.Remove(entry);
                        context.SaveChanges();
                    }
                }
            }

            public bool IsWordInList(string word, bool caseSensetive) {
                using (var context = new DatabaseContext()) {
                    return context.BlacklistedWords.Any(w => string.Compare(word, w.Word, caseSensetive) == 0);
                }
            }

            public bool IsBlacklistedWordInMessage(string message) {
                using (var context = new DatabaseContext()) {
                    return context.BlacklistedWords.Any(word => message.Contains(word.Word));
                }
            }
        }
    }
}