using StockQuoteAlert;

if (args.Length != 3) {
    Console.WriteLine("Uso correto: programa <Ativo> <PrecoVenda> <PrecoCompra>");
    return;
}

string ativo = args[0].ToUpper(); // Para padronizar o nome do ativo em maiúsculo

bool deuCertoVenda = decimal.TryParse(args[1], out decimal precoVenda);
bool deuCertoCompra = decimal.TryParse(args[2], out decimal precoCompra);

if (!deuCertoCompra || !deuCertoVenda) { // Se não foi possível converter algum dos dois para decimal
    Console.WriteLine("Uso correto: Preço de compra e venda devem ser números");
    return;
}

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton(new AlertSettings {
    Ativo = ativo,
    PrecoVenda = precoVenda,
    PrecoCompra = precoCompra
});

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings")); // Dados do appsettings.json

builder.Services.AddHttpClient<MarketService>(); // Sistema reconhece a classe MarketService

builder.Services.AddTransient<EmailService>(); // AddTransient pois email é um serviço leve (pode criar um novo toda vez que for usar sem problemas)

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();

public class AlertSettings {
    public string? Ativo { get; set; } // o ? é para transformar a variável em um Nullable Type e remover os alertas
    public decimal PrecoVenda { get; set; }
    public decimal PrecoCompra { get; set; }
}

public class EmailSettings {
    public string? ServidorSmtp { get; set; }
    public int Porta { get; set; }
    public string? EmailOrigem { get; set; }
    public string? Senha { get; set; }
    public string? EmailDestino { get; set; }
}
