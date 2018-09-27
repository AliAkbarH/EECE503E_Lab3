using Microsoft.WindowsAzure.Storage.Table;

namespace ChatService.Storage.Azure
{
    public class MessageTableEntity:TableEntity
    {
        public MessageTableEntity()
        {
            //default Constructor
        }

        public string MessageBody { set; get; }
        
    }
}