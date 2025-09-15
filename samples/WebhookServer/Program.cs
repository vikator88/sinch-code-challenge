using DevexpApiSdk.Common.Security;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string secret = "mySecret"; // en real, vendría de configuración

app.MapPost(
    "/webhooks",
    async (HttpRequest request, HttpResponse response) =>
    {
        // 1. Leer el body
        using var reader = new StreamReader(request.Body);
        var body = await reader.ReadToEndAsync();

        // 2. Leer header Authorization
        if (!request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        // 3. Verificar firma usando el SDK
        var valid = WebhookVerifier.VerifySignature(body, authHeader!, secret);
        if (!valid)
        {
            response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        // 4. Procesar evento
        Console.WriteLine("Received webhook event:");
        Console.WriteLine(body);

        response.StatusCode = StatusCodes.Status200OK;
    }
);

app.Run("http://localhost:3010");
