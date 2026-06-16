namespace OnnxExperimentation.Maui;

using Inference;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

public class MainPageViewModel : INotifyPropertyChanged
{
    private readonly IInferenceServiceFactory _inferenceServiceFactory;
    private IInferenceService? _inferenceService;
    private CancellationTokenSource? _cts;

    public string Prompt
    {
        get;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    } = string.Empty;

    public string Response
    {
        get;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    } = string.Empty;

    public bool IsBusy
    {
        get;
        set
        {
            field = value;
            OnPropertyChanged();
            ((Command)SendCommand).ChangeCanExecute();
        }
    }

    public ICommand SendCommand { get; }

    public MainPageViewModel(IInferenceServiceFactory inferenceServiceFactory)
    {
        _inferenceServiceFactory = inferenceServiceFactory;

        SendCommand = new Command(async () => await Send(), () => !IsBusy);
    }

    private async Task Send()
    {
        if (string.IsNullOrWhiteSpace(Prompt))
            return;

        IsBusy = true;
        Response = string.Empty;

        _cts?.Cancel();
        _cts = new CancellationTokenSource();

        try
        {
            await Task.Run(async () =>
            {
                var inferenceService = await GetInferenceService();
                foreach (var chunk in inferenceService.Prompt(Prompt, _cts.Token))
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        Response += chunk;
                    });
                }
            });
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task<IInferenceService> GetInferenceService()
    {
        _inferenceService ??= await _inferenceServiceFactory.Build();

        return _inferenceService;
    }

    public void Cancel()
    {
        _cts?.Cancel();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = "")
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}