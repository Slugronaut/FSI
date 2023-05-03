using System.Windows.Forms;
using Toolbox;

namespace SAOT
{
    public class FilterUpdatedMessage : IMessage
    {
        public readonly Form Parent;

        public FilterUpdatedMessage(Form parent)
        {
            Parent = parent;
        }
    }
}
