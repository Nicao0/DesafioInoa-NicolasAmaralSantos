namespace StockQuoteAlert;

public class Worker : BackgroundService { 

    private readonly AlertSettings _alertSettings; // "memória", sabe o nome do ativo e os preços digitados
    private readonly MarketService _marketService; // traz da internet o preço da ação
    private readonly EmailService _emailService; // dá a ordem de disparar o email

    public Worker(AlertSettings alertSettings, MarketService marketService, EmailService emailService) {
        _alertSettings = alertSettings;
        _marketService = marketService;
        _emailService = emailService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {

        while (!stoppingToken.IsCancellationRequested) {  

            decimal precoAtual = await _marketService.GetPrice(_alertSettings.Ativo);

            if (precoAtual >= _alertSettings.PrecoVenda) {
                await _emailService.EnviarEmail("Sinal de venda", "Ação atingiu a meta, hora de vender.");

            } else if (precoAtual <= _alertSettings.PrecoCompra) {
                await _emailService.EnviarEmail("Sinal de compra", "Ação baixou de preço, hora de comprar");
            }

            await Task.Delay(600000, stoppingToken); // Repete o loop a cada 10 minutos
        }
    }
}
