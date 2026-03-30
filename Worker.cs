namespace StockQuoteAlert;


public class Worker : BackgroundService { 

    private readonly AlertSettings _alertSettings; // "memória", sabe o nome do ativo e os preços digitados
    private readonly MarketService _marketService; // traz da internet o preço da ação
    private readonly EmailService _emailService; // dá a ordem de disparar o email
    private readonly ILogger<Worker> _logger; // ferramenta de logs do .NET

    private decimal _ultimoPrecoConhecido = 0; // memória para o caso da API cair, usar o último resultado

    public Worker(AlertSettings alertSettings, MarketService marketService, EmailService emailService, ILogger<Worker> logger) {
        _alertSettings = alertSettings;
        _marketService = marketService;
        _emailService = emailService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        
        _logger.LogInformation("=================================================");
        _logger.LogInformation($"Iniciando monitoramento de {_alertSettings.Ativo}");
        _logger.LogInformation($"Alvo de Venda: R$ {_alertSettings.PrecoVenda} | Alvo de Compra: R$ {_alertSettings.PrecoCompra}");
        _logger.LogInformation("=================================================");

        while (!stoppingToken.IsCancellationRequested) {

            try {
                _logger.LogInformation("Consultando a Brapi...");
    
                decimal precoAtual = await _marketService.GetPrice(_alertSettings.Ativo!);
                _ultimoPrecoConhecido = precoAtual;

                _logger.LogInformation($"Preço atual: R$ {precoAtual}");

                if (precoAtual >= _alertSettings.PrecoVenda) {
                    _logger.LogInformation("Sinal de VENDA detectado. Tentando enviar e-mail...");
                    await _emailService.SendEmail("Sinal de venda", "Ação atingiu a meta, hora de vender.");
                    _logger.LogInformation("E-mail enviado com sucesso.");

                } else if (precoAtual <= _alertSettings.PrecoCompra) {
                    _logger.LogInformation("Sinal de COMPRA detectado. Tentando enviar e-mail...");
                    await _emailService.SendEmail("Sinal de compra", "Ação baixou de preço, hora de comprar");
                    _logger.LogInformation("E-mail enviado com sucesso.");
                }


            } catch (Exception exception) {
                _logger.LogError($"[FALHA] Ocorreu um erro no ciclo atual: {exception.Message}");

                if (_ultimoPrecoConhecido > 0) {
                    _logger.LogWarning($"Mantendo o último preço conhecido: R$ {_ultimoPrecoConhecido}. O sistema não será desligado.");
                } else {
                    _logger.LogWarning("Nenhum preço foi registrado ainda. O sistema não será desligado.");
                }
            }

            _logger.LogInformation("Aguardando o próximo ciclo...");
            await Task.Delay(600000, stoppingToken); // Repete o loop a cada 10 minutos
        }
    }
}
