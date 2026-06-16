namespace OnnxExperimentation.Inference;

public interface IInferenceService
{
    IEnumerable<string> Prompt(string prompt, CancellationToken ct);
}