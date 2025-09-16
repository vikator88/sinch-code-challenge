using DevexpApiSdk.Samples;

var cts = new CancellationTokenSource();

// Cancel on Ctrl+C or SIGTERM
Console.CancelKeyPress += (sender, e) =>
{
    e.Cancel = true; // prevent immediate process termination
    cts.Cancel();
};

AppDomain.CurrentDomain.ProcessExit += (sender, e) => cts.Cancel();

var cancellationToken = cts.Token;

if (args.Length == 0)
{
    Console.WriteLine(
        "Please specify a sample to run: contacts | multi-contacts | messages | logger | profiler"
    );
    return;
}

var sampleName = args[0].ToLowerInvariant();

switch (sampleName)
{
    case "contacts":
        await ContactCrudSample.RunAsync(cancellationToken);
        break;
    case "multi-contacts":
        await ContactCrudMultiSample.RunAsync(cancellationToken);
        break;
    case "messages":
        await MessageSample.RunAsync(cancellationToken);
        break;
    case "logger":
        await WithLoggerSample.RunAsync(cancellationToken);
        break;
    case "profiler":
        await WithLoggerProfilerSample.RunAsync(cancellationToken);
        break;

    default:
        Console.WriteLine(
            $"Unknown sample '{sampleName}'. Available: contacts | multi-contacts | messages | logger | profiler"
        );
        break;
}
