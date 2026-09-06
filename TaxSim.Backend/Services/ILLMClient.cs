namespace TaxSim.Backend.Services
{
    public interface ILLMClient {
       Task<String> GenerateAsync(string prompt); 
    }
}