using DevexpApiSdk.Samples;

class Program
{
    static async Task Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Please specify a sample to run: contacts | messages | bulk");
            return;
        }

        var sampleName = args[0].ToLowerInvariant();

        switch (sampleName)
        {
            case "contacts":
                await ContactsManagementSample.RunAsync();
                break;
            case "messages":
                await MessageSenderSample.RunAsync();
                break;

            default:
                Console.WriteLine($"Unknown sample '{sampleName}'. Available: contacts | messages");
                break;
        }
    }
}
