using System.Collections.Generic; // Para usar o List
using System.Net.Http; // Para o HttpClient
using System.Net.Http.Json;

namespace StockQuoteAlert;

public class MarketService {
    private readonly HttpClient _httpClient;
    public MarketService(HttpClient httpClient) {
        _httpClient = httpClient;
    }

    public async Task<decimal> GetPrice(string nomeAcao) {
        string url = $"https://brapi.dev/api/quote/{nomeAcao}";
        var resultado = await _httpClient.GetFromJsonAsync<ApiResponse>(url); // Espera o resultado da internet e encaixa na classe ApiResponse
        
        if (resultado?.Results == null || resultado.Results.Count == 0) {
            throw new Exception($"Ativo '{nomeAcao}' não encontrado na Brapi.");
        }

        return resultado.Results[0].RegularMarketPrice; // Entrando na lista que a Brapi enviou e pegando o primeiro e único item que é o preço
    }
}
