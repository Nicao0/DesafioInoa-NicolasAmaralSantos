namespace StockQuoteAlert;

public class AlertSettings {
    public string? Ativo { get; set; } // o ? é para transformar a variável em um Nullable Type e remover os alertas
    public decimal PrecoVenda { get; set; }
    public decimal PrecoCompra { get; set; }
}