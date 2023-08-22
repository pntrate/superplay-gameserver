namespace SuperPlay.Game.Application.Modules.Player.Models.Common
{
    [MessageType("ErrorMessage")]
    public class ErrorMessage
    {
        public string Text { get; set; }

        public ErrorMessage()
        {

        }

        public ErrorMessage(string text)
        {
            Text = text;
        }

        public ErrorMessage(string text, Exception? ex = null)
        {
            Text = text;
            if (ex is not null)
            {
                Text += $". InnerException {ex.Message}";
            }
        }
    }
}