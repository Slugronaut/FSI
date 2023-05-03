using Toolbox;

namespace SAOT
{
    public class WarehouseCreatedMessage : IMessage
    {
        public readonly string Name;

        public WarehouseCreatedMessage(string name)
        {
            Name = name;
        }
    }
}
