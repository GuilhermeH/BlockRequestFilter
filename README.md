# ⏳ BlockRequestFilter

Este repositório demonstra como implementar um filtro personalizado no ASP.NET Core para **bloquear requisições consecutivas** realizadas em um curto intervalo de tempo a partir do mesmo IP para o mesmo endpoint.

A ideia é evitar sobrecarga em endpoints sensíveis, reduzir spam ou mitigar tentativas de ataque de força bruta.

## 💡 Visão geral

A classe `BlockImmediateRequestFilter` implementa a interface `IActionFilter` e utiliza `IMemoryCache` para armazenar temporariamente um identificador do cliente (IP + endpoint) por um período definido (padrão: 5 segundos).

Se uma nova requisição for detectada antes do tempo expirar, a requisição será bloqueada com status HTTP **429 (Too Many Requests)**.

## 🛠️ Como usar

1. **Registrar o serviço de cache de memória** no `Program.cs` ou `Startup.cs`:

```csharp
builder.Services.AddMemoryCache();
```

2. **Adicionar o filtro globalmente ou por ação/controlador**, por exemplo:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers(options =>
    {
        options.Filters.Add<BlockImmediateRequestFilter>();
    });

    services.AddScoped<BlockImmediateRequestFilter>();
    services.AddMemoryCache();
}
```

Ou aplicar via atributo em um controller específico:

```csharp
[ServiceFilter(typeof(BlockImmediateRequestFilter))]
[HttpGet]
public class TestController : ControllerBase
{
    [ServiceFilter(typeof(BlockImmediateRequestFilter))]
    [HttpGet]
    public string Get()
    {
        return "Request Accepted";
    }
}
```

## ⚠️ Limitações

Este projeto foi criado para **fins educativos e aplicações de pequeno porte**. Como utiliza `IMemoryCache`, o cache é **local** a cada instância da aplicação.

### Problemas em cenários com múltiplas instâncias:
- O bloqueio não se propaga entre servidores ou containers
- Pode gerar comportamento inconsistente (rejeição em uma instância, aceitação em outra)
- Sem observabilidade ou persistência entre reinícios da aplicação

## ✅ Para ambiente produtivos e/ou de larga escala utilize um **cache distribuído**, como o **Redis**



## 📂 Estrutura do projeto

```
BlockRequestFilter/
├── BlockImmediateRequestFilter.cs
├── Controllers/
│   └── TestController.cs
├── Program.cs
└── ...
```
