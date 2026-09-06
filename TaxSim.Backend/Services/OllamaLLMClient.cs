using System;
using System.Text;
using System.Threading.Tasks;
using OllamaSharp;

namespace TaxSim.Backend.Services
{
    public class OllamaLLMClient : ILLMClient
    {
        private readonly OllamaApiClient _client;

        public OllamaLLMClient()
        {
            _client = new OllamaApiClient(new Uri("http://localhost:11434/"), "llama3.2");
        }

        public async Task<string> GenerateAsync(string prompt)
        {
            var sb = new StringBuilder();

            // OllamaSharp streams responses back, so we use await foreach to capture every chunk
            await foreach (var stream in _client.GenerateAsync(new OllamaSharp.Models.GenerateRequest 
            { 
                Model = "llama3.2", 
                Prompt = prompt 
            }))
            {
                if (stream?.Response != null)
                {
                    sb.Append(stream.Response);
                }
            }

            var result = sb.ToString();
            return string.IsNullOrWhiteSpace(result) ? "{}" : result;
        }
    }
}