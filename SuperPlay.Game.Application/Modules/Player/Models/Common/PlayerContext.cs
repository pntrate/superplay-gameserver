﻿using SuperPlay.Game.Application.Modules.Player.Hub;

namespace SuperPlay.Game.Application.Modules.Player.Models.Common
{
    public class PlayerContext
    {
        public Guid PlayerId { get; set; }
        public PlayerChannel? PlayerChannel { get; set; }
    }
}