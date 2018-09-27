using System.Collections.Generic;
using System.Threading.Tasks;

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

        public Task AddConversation(Conversation conversation)
        {
            throw new System.NotImplementedException();
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