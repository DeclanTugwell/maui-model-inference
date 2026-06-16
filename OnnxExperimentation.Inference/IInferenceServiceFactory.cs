namespace OnnxExperimentation.Inference
{
    public interface IInferenceServiceFactory
    {
        Task<IInferenceService> Build();
    }
}
