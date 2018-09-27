using Microsoft.WindowsAzure.Storage.Table;

namespace ChatService.Storage.Azure
{
    public class ConversationTableEntity: TableEntity
    {
        public ConversationTableEntity()
        {//Default constructor 

        }
        public string lastModified { set; get; }
    }
}