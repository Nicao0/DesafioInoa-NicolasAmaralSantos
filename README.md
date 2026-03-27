# Stock Quote Alert

Sistema de monitoramento de ativos financeiros desenvolvido em **.NET 8**. O software consulta preços em tempo real via API, compara com limites de preço definidos pelo usuário e envia notificações automáticas por e-mail.

---

## Como Configurar

Para que o sistema consiga enviar notificações, você deve configurar suas credenciais no arquivo **`appsettings.json`** localizado na raiz da pasta:

* **EmailOrigem**: Seu endereço de e-mail (Gmail recomendado).
* **Senha**: Sua **Senha de Aplicativo** (No Gmail: *Segurança > Verificação em duas etapas > Senhas de app*).
* **EmailDestino**: O e-mail que receberá os alertas.
* **ServidorSmtp**: Por padrão, está configurado para `smtp.gmail.com`.

---

## Como Executar

O programa deve ser executado via terminal (CMD ou PowerShell) passando três argumentos obrigatórios: **Ativo**, **Preço de Venda** e **Preço de Compra**.

### Exemplo de comando:

```bash
.\stock-quote-alert.exe PETR4 22.67 22.59 
```
* **PETR4:** Ativo a ser monitorado (padrão Brapi).
* **22.67:** Preço acima do qual o sistema enviará um alerta de venda.
* **22.59:** Preço abaixo do qual o sistema enviará um alerta de compra.