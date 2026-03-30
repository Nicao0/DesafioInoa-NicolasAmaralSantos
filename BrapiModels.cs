using System.Collections.Generic;

namespace StockQuoteAlert;

public class StockInfo {
    public decimal RegularMarketPrice { get; set; } // Preço da ação
}

public class ApiResponse {
    public List<StockInfo>? Results { get; set; }
}