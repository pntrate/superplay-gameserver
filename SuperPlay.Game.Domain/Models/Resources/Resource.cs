namespace SuperPlay.Game.Domain.Models.Resources
{
    public class Resource : ValueObject
    {
        public ResourceType Type { get; private set; }
        public int Count { get; private set; }

        private Resource(ResourceType type, int count)
        {
            Type = type;
            Count = count;
        }

        public static Resource Create(ResourceType type, int count)
        {
            return new Resource(type, count);
        }

        public void AddCount(int count)
        {
            Count += count;
        }

        public void SetCount(int count)
        {
            Count = count;
        }
    }
}