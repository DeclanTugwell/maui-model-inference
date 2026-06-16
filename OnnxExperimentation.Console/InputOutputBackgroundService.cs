using Microsoft.Extensions.Hosting;
using OnnxExperimentation.Inference;

namespace OnnxExperimentation.Console;

public class InputOutputBackgroundService(IInferenceService inferenceService) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (stoppingToken.IsCancellationRequested == false)
        {
            var prompt = FetchInput("Prompt:");
            foreach (var token in inferenceService.Prompt(prompt, stoppingToken))
            {
                System.Console.Write(token);
            }

            System.Console.WriteLine(Environment.NewLine);
        }

        return Task.CompletedTask;
    }

    private string FetchInput(string message)
    {
        System.Console.WriteLine(message + Environment.NewLine);
        var input = string.Empty;

        while (string.IsNullOrEmpty(input))
        {
            input = System.Console.ReadLine();
        }

        return input;
    }
}