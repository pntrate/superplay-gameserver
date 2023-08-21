using SuperPlay.Game.Domain.Common.Exceptions;
using SuperPlay.Game.Domain.Common.Helpers;
using SuperPlay.Game.Domain.Models.Resources;

namespace SuperPlay.Game.Domain.Models.Players
{
    public class Player : AggregateRoot<Guid>
    {
        public bool IsOnline { get; private set; }
        public DateTime? LastLoginDate { get; private set; }
        public Guid DeviceId { get; private set; }

        public List<Resource> Resources { get; private set; }

        private Player(Guid deviceId)
        {
            Id = Guid.NewGuid();
            DeviceId = deviceId;
            Resources = new();
        }

        public static Player Create(Guid deviceId)
        {
            return new Player(deviceId);
        }

        public void Login()
        {
            if (IsOnline)
            {
                throw new DomainException($"Player {Id} with DeviceId {DeviceId} is already logged in");
            }

            IsOnline = true;
            LastLoginDate = DateTimeProvider.Now;
        }

        public void SendGift(ResourceType resourceType, int value)
        {
            if (Enum.IsDefined(typeof(ResourceType), resourceType) || resourceType == ResourceType.None)
            {
                throw new DomainException("");
            }

            if (value < 0)
            {
                throw new DomainException("");
            }

            var resource = Resources.FirstOrDefault(i => i.Type == resourceType);
            if (resource is Resource)
            {
                var newBalance = resource.Balance - value;
                if (newBalance < 0)
                {
                    throw new DomainException("");
                }
                resource.SetBalance(newBalance);
            }
            else
            {
                throw new DomainException("");
            }
        }

        public int UpdateResources(ResourceType resourceType, int value)
        {
            if (Enum.IsDefined(typeof(ResourceType), resourceType) || resourceType == ResourceType.None)
            {
                throw new DomainException("");
            }

            if (value < 0)
            {
                throw new DomainException("");
            }

            var resource = Resources.FirstOrDefault(i => i.Type == resourceType);
            if (resource is Resource)
            {
                resource.AddBalance(value);
            }
            else
            {
                resource = Resource.Create(resourceType, value);
                Resources.Add(resource);
            }
            return resource.Balance;
        }
    }
}