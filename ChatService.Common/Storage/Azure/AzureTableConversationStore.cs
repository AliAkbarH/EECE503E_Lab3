using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace ChatService.Storage.Azure
{
    public class AzureTableConversationStore : IConversationsStore
    {
        private readonly ICloudTable ConversationsTable;
        private readonly ICloudTable MessagesTable;

        public AzureTableConversationStore(ICloudTable conversations, ICloudTable messages)
        {
            ConversationsTable = conversations;
            MessagesTable = messages;
        }

        public async Task AddConversationAsync(Conversation conversation)
        {
            var entity1=new ConversationTableEntity
            {
                RowKey = conversation.Id,
                PartitionKey = conversation.Participants[0],
                lastModified = conversation.LastModifiedDateUtc.ToString()

            };
            var entity2 = new ConversationTableEntity
            {
                RowKey = conversation.Id,
                PartitionKey = conversation.Participants[1],
                lastModified = conversation.LastModifiedDateUtc.ToString()

            };//assuming only 2 participants per chat, can work with a list and iterate over participants if we want to make it more general

            var insertOperation1 = TableOperation.Insert(entity1);
            var insertOperation2 = TableOperation.Insert(entity2);

            try
            {
                await ConversationsTable.ExecuteAsync(insertOperation1);
                
            }
            catch (StorageException e)
            {
                if (e.RequestInformation.HttpStatusCode == 409) // not found
                {
                    throw new DuplicateProfileException
                        ($"A conversation between {conversation.Participants[0]} and {conversation.Participants[1]} already exists");
                }
                throw new StorageErrorException("Could not write to Azure Table", e);
            }
            try
            {
                await ConversationsTable.ExecuteAsync(insertOperation2);

            }
            catch (StorageException e)
            {
                if (e.RequestInformation.HttpStatusCode == 409) // not found
                {
                    throw new DuplicateProfileException
                        ($"A conversation between {conversation.Participants[0]} and {conversation.Participants[1]} already exists");
                }
                throw new StorageErrorException("Could not write to Azure Table", e);
            }


        }

        public Task AddMessage(string conversationId, Message message)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Conversation>> ListConversations(string username)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Message>> ListMessages(string conversationId)
        {
            throw new System.NotImplementedException();
        }
    }
}