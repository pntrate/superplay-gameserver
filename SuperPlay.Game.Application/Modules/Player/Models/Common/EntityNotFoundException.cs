namespace SuperPlay.Game.Domain.Common.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string message) : base(message)
        {

        }

        public EntityNotFoundException(string message, Exception ex) : base(message, ex)
        {

        }
    }
}
