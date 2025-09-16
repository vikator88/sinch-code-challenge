using DevexpApiSdk.Common.Security;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Just for testing. In production, use a secure secret and store it safely
const string secret = "mySecret";

app.MapPost(
    "/webhooks",
    async (HttpRequest request, HttpResponse response) =>
    {
        // Read the request body
        using var reader = new StreamReader(request.Body);
        var body = await reader.ReadToEndAsync();

        // 2. Read the Authorization header
        if (!request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        // 3. Verify using SDK
        var valid = WebhookVerifier.VerifySignature(body, authHeader!, secret);
        if (!valid)
        {
            response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        // 4. Process the event (just print it here)
        Console.WriteLine("Received webhook event:");
        Console.WriteLine(body);

        response.StatusCode = StatusCodes.Status200OK;
    }
);

app.Run("http://localhost:3010");
