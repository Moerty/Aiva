namespace AivaBot.Bankheist.Models {
    public class BankheistModel {
        public string name;
        public int value;

        public BankheistModel(string name, int value) {
            this.name = name;
            this.value = value;
        }

        public class Enums {
            public enum BankheistStatus {
                IsActive,
                OnCooldown,
                Ready
            }
        }
    }
}