using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnnxExperimentation.Console;
using OnnxExperimentation.Inference;

var config = new ModelConfiguration(@"D:\ai_models\gemma-3-1b", new PhiChatTemplate());

var appHost = new HostBuilder()
    .ConfigureServices(collection =>
    {
        collection.AddSingleton(config);
        collection.AddScoped<IInferenceService, InferenceService>();
        collection.AddHostedService<InputOutputBackgroundService>();
    })
    .Build();

appHost.Run();