namespace StockQuoteAlert;

public class EmailSettings {
    public string? ServidorSmtp { get; set; }
    public int Porta { get; set; }
    public string? EmailOrigem { get; set; }
    public string? Senha { get; set; }
    public string? EmailDestino { get; set; }
}