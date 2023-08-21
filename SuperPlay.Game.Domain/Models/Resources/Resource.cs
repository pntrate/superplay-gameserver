namespace SuperPlay.Game.Domain.Models.Resources
{
    public class Resource : ValueObject
    {
        public ResourceType Type { get; private set; }
        public int Balance { get; private set; }

        private Resource(ResourceType type, int balance)
        {
            Type = type;
            Balance = balance;
        }

        public static Resource Create(ResourceType type, int balance)
        {
            return new Resource(type, balance);
        }

        public void AddBalance(int value)
        {
            Balance += value;
        }

        public void SetBalance(int value)
        {
            Balance = value;
        }
    }
}