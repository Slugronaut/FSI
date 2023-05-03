using Toolbox;

namespace SAOT
{
    /// <summary>
    /// Means of telling the application that an unrecoverable error has occured and that it should post any relevant details before shutting down.
    /// </summary>
    public class FatalErrorMessage : IMessage
    {
        public string Details { get; private set; }

        public FatalErrorMessage(string details = null)
        {
            Details = details;
        }
    }
}
