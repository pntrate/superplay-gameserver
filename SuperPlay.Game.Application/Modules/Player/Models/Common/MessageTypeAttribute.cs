namespace SuperPlay.Game.Application.Modules.Player.Models.Common
{
    public class MessageTypeAttribute : Attribute
    {
        public string Name { get; private set; }

        public MessageTypeAttribute(string name)
        {
            Name = name;
        }
    }
}