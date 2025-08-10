using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dev_library.Data;
using OpenAI;
using OpenAI.Chat;

namespace dev_library.Clients
{
    public class AiClient
    {
        public async Task<string> GetResponse(string message, int aggresionLevel)
        {
            var client = new ChatClient("gpt-5-nano", AppSettings.GptSettings.ApiToken);

            message = message.Replace("<@1305976068053794846>", "");

            var suffix = $"The level is {aggresionLevel}";

            ChatCompletion completion = client.CompleteChat($"{AppSettings.GptSettings.Prefix} {message} {AppSettings.GptSettings.Suffix} {aggresionLevel}");

            return completion.Content[0].Text.Replace("@Refined Bot", "");
        }
    }
}
