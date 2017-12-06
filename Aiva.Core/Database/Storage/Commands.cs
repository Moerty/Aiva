namespace Aiva.Core.Database.Storage {
    public class Commands {
        public int CommandsId { get; set; }

        public string Name { get; set; }
        public string Text { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public string CreatedFrom { get; set; }
        public System.DateTime? ModifiedAt { get; set; }
        public string ModifiedFrom { get; set; }
        public long? Stack { get; set; }
        public System.DateTime? LastExecution { get; set; }
    }
}