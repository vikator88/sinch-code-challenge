# DevexpApiSdk

A lightweight .NET SDK to interact with the API provided by Sinch.
Includes examples, retry logic with Polly, logging hooks, and metrics profiling.

---

## Getting Started

### 1. Starting the API

The test environment provides a docker-compose file with the required services.

```bash
docker compose up
```

This will start the API provided by Sinch (default: `http://localhost:3000`).

### 2. Run the webhook server

The SDK includes a simple **Minimal API** server that listens for webhook callbacks from the API

```bash
dotnet run --project samples/WebhookServer
```

By default it listens on:

```
http://localhost:3010/webhooks
```

---

### 3. Execute the examples

The `DevexpConsoleSample` project demonstrates how to CRUD contacts, send messages and handle responses.
Also has some samples to show how to attach a profiler hook or a Logger that implements ILogger from Microsoft.

An argument is needed to choose what sample you need to test. This are the available values:

```
contacts | multi-contacts | messages | logger | profiler
```

Executing the `messages` sample with the WebhookServer running will show a message in WebhookServer's console like this:

```

Received webhook event:
{
"messageId": "b8c2e7b1-7cd4-4e6c-a302-08d9a7f2d17e",
"status": "queued",
"deliveredAt": null
}

```

---

## SDK Configuration (DevexpApiOptions)

The `DevexpApiOptions` record centralizes all SDK configuration and needs to be setted with the `DevexpApiOptionsBuilder`

```csharp
var options = new DevexpApiOptionsBuilder()
 .WithApiKey("your-api-key")
 .WithTimeout(TimeSpan.FromSeconds(30))
 .EnableRetries() // Polly retries enabled by default
 .EnableLogging(logger) // Plug in any Microsoft ILogger
 .WithOperationProfiler(metric =>
 {
 // Custom profiler hook
 Console.WriteLine($"{metric.OperationName} took {metric.Duration}");
 })
 .Build();
```

### Retries with Polly

- **Enabled by default** (`EnableAutoRetries = true`).
- Retries on transient failures (configurable max attempts and delay).
- Based on `Polly` library.
- Can be disabled if the developer prefers to manage retries by itself.

### Logging

- Compatible with any implementation of `ILogger` from `Microsoft.Extensions.Logging`.
- Example with console logger:

```csharp
using Microsoft.Extensions.Logging;
using var loggerFactory = LoggerFactory.Create(builder =>
{
 builder.AddConsole();
});
var logger = loggerFactory.CreateLogger("DevexpApiSdk");
var options = new DevexpApiOptionsBuilder()
 .WithApiKey("your-api-key")
 .EnableLogging(logger)
 .Build();
```

### Profiler (Metrics Hook)

- Provide a callback `Action<OperationPerformanceMetric>` to capture execution metrics.
- Useful for integrating with monitoring systems (Prometheus, Application Insights, etc.).

#### Example with Prometheus

```csharp
using Prometheus;
var durationHistogram = Metrics.CreateHistogram(
 "devexp_operation_duration_seconds",
 "Duration of Devexp SDK operations",
 new HistogramConfiguration
 {
 LabelNames = new[] { "operation", "success" }
 });
var options = new DevexpApiOptionsBuilder()
 .WithApiKey("your-api-key")
 .WithOperationProfiler(metric =>
 {
 durationHistogram
 .WithLabels(metric.OperationName, metric.Success.ToString())
 .Observe(metric.Duration.TotalSeconds);
 })
 .Build();
```

---

## Running the Samples

1. Start docker services:

```bash
docker compose up
```

2. Start webhook server:

```bash
dotnet run --project samples/WebhookServer
```

3. Run example console app:

```bash
dotnet run --project samples/DevexpConsoleSample [sampleId]
```

---

## Notes

- Default API base URL: `http://localhost:3000` (can be overridden in options).

---
