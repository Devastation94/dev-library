using dev_library.Data;
using OpenAI;
using OpenAI.Chat;

namespace dev_library.Clients
{
    public class AiClient
    {
        private ChatClient _client = new ChatClient("gpt-5-nano", AppSettings.GptSettings.ApiToken);
        private List<ChatMessage> _history = new List<ChatMessage>
            {
                new SystemChatMessage(AppSettings.GptSettings.Prefix)
            };

        public async Task<string> GetResponse(string message, int aggressionLevel)
        {
            // Clean up mention
            message = message.Replace("<@1305976068053794846>", "").Trim();

            // Add user message
            _history.Add(new UserChatMessage(message));

            // Send the entire conversation so far
            ChatCompletion completion = await _client.CompleteChatAsync(_history);

            var reply = completion.Content[0].Text
                .Replace("@Refined Bot", "")
                .Trim();

            // Add assistant reply
            _history.Add(new AssistantChatMessage(reply));

            return reply;
        }
    }
}
