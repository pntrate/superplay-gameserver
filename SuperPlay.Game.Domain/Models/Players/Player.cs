using SuperPlay.Game.Domain.Models.Resources;

namespace SuperPlay.Game.Domain.Models.Players
{
    public class Player : AggregateRoot<long>
    {
        public bool IsOnline { get; private set; }
        public DateTime? LastLoginDate { get; private set; }
        public Guid? DeviceId { get; private set; }

        public List<Resource> Resources { get; private set; }

        private Player(long id, Guid deviceId, bool isOnline)
        {
            Id = id;

            DeviceId = deviceId;
            IsOnline = isOnline;
            
            Resources = new();
        }

        public static Player Create(long id, Guid deviceId, bool isOnline)
        {
            return new Player(id, deviceId, isOnline);
        }
    }
}