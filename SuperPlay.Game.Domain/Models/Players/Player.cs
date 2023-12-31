﻿using SuperPlay.Game.Domain.Common.Exceptions;
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

        //Public Constructor just for testing purposes (to seed few players)
        public Player(Guid id, Guid deviceId)
        {
            Id = id;
            DeviceId = deviceId;
            Resources = new List<Resource>();
        }

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

        public void SendGift(ResourceType resourceType, int resourceValue)
        {
            if (!Enum.IsDefined(typeof(ResourceType), resourceType) || resourceType == ResourceType.None)
            {
                throw new DomainException("ResourceType not defined");
            }

            if (resourceValue < 0)
            {
                throw new DomainException("ResourceValue should be positive integer");
            }

            var resource = Resources.FirstOrDefault(i => i.Type == resourceType);
            if (resource is Resource)
            {
                var newBalance = resource.Balance - resourceValue;
                if (newBalance < 0)
                {
                    throw new DomainException("Balance cannot be negative");
                }
                resource.SetBalance(newBalance);
            }
            else
            {
                throw new DomainException("Resource not found");
            }
        }

        public Resource UpdateResources(ResourceType resourceType, int resourceValue)
        {
            if (!Enum.IsDefined(typeof(ResourceType), resourceType) || resourceType == ResourceType.None)
            {
                throw new DomainException("ResourceType not defined");
            }

            if (resourceValue < 0)
            {
                throw new DomainException("ResourceValue should be positive integer");
            }

            var resource = Resources.FirstOrDefault(i => i.Type == resourceType);
            if (resource is Resource)
            {
                resource.AddBalance(resourceValue);
            }
            else
            {
                resource = Resource.Create(resourceType, resourceValue);
                Resources.Add(resource);
            }
            return resource;
        }
    }
}