using OnnxExperimentation.Inference;

namespace OnnxExperimentation.Maui
{
    public class InferenceServiceFactory : IInferenceServiceFactory
    {
        public async Task<IInferenceService> Build()
        {
            var modelConfig = new ModelConfiguration(await ModelLoader.PrepareModelAsync(), new GemmaChatTemplate());
            return new InferenceService(modelConfig);
        }
    }
}
