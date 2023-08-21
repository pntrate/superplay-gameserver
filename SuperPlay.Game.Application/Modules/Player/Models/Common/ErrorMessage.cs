namespace SuperPlay.Game.Application.Modules.Player.Models.Common
{
    [MessageType("ErrorMessage")]
    public class ErrorMessage
    {
        public string Text { get; private set; }

        public ErrorMessage(string text)
        {
            Text = text;
        }
    }
}