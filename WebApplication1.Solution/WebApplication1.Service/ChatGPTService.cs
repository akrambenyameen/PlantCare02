using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using OpenAI_API;


namespace WebApplication1.Service
{
    public class ChatGPTService
    {
        private readonly OpenAIAPI api;

        
        public ChatGPTService(string apiKey)
        {
            api = new OpenAIAPI(apiKey);
        }

        public async Task<string> GetResponseAsync(string question)
        {
            var chatRequest = new OpenAI_API.Chat.ChatRequest()
            {
                Model = OpenAI_API.Models.Model.ChatGPTTurbo,
                Messages = new List<OpenAI_API.Chat.ChatMessage>
                {
                    new OpenAI_API.Chat.ChatMessage(role:OpenAI_API.Chat.ChatMessageRole.User,question)
                }
            };
            var response = await api.Chat.CreateChatCompletionAsync(chatRequest);
            return response.Choices[0].Message.TextContent;
        }
    }
}
