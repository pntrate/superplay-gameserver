using SuperPlay.Game.Domain.Common.Exceptions;
using SuperPlay.Game.Domain.Common.Helpers;
using SuperPlay.Game.Domain.Models.Resources;

namespace SuperPlay.Game.Domain.Models.Players
{
    public class Player : AggregateRoot<Guid>
    {
        public string Name { get; private set; }
        public bool IsOnline { get; private set; }
        public DateTime? LastLoginDate { get; private set; }
        public Guid DeviceId { get; private set; }

        public List<Resource> Resources { get; private set; }

        private Player(Guid deviceId, string name)
        {
            Id = Guid.NewGuid();
            DeviceId = deviceId;
            Name = name;
            Resources = new();
        }

        public static Player Create(Guid deviceId, string name)
        {
            return new Player(deviceId, name);
        }

        public void Login()
        {
            IsOnline = true;
            LastLoginDate = DateTimeProvider.Now;
        }

        public void SendGift(ResourceType resourceType, int count)
        {
            if (Enum.IsDefined(typeof(ResourceType), resourceType) || resourceType == ResourceType.None)
            {
                throw new DomainException("");
            }

            if (count < 0)
            {
                throw new DomainException("");
            }

            var resource = Resources.FirstOrDefault(i => i.Type == resourceType);
            if (resource is Resource)
            {
                var newCount = resource.Count - count;
                if (newCount < 0)
                {
                    throw new DomainException("");
                }
                resource.SetCount(newCount);
            }
            else
            {
                throw new DomainException("");
            }
        }

        public void ReceiveGift(ResourceType resourceType, int count)
        {
            if (Enum.IsDefined(typeof(ResourceType), resourceType) || resourceType == ResourceType.None)
            {
                throw new DomainException("");
            }

            if (count < 0)
            {
                throw new DomainException("");
            }

            var resource = Resources.FirstOrDefault(i => i.Type == resourceType);
            if (resource is Resource)
            {
                resource.AddCount(count);
            }
            else
            {
                resource = Resource.Create(resourceType, count);
                Resources.Add(resource);
            }
        }
    }
}