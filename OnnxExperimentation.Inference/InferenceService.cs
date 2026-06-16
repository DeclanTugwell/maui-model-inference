using Microsoft.ML.OnnxRuntimeGenAI;

namespace OnnxExperimentation.Inference
{
    public class InferenceService : IInferenceService, IDisposable
    {
        private readonly OgaHandle _ogaHandle;
        private readonly Model _model;
        private readonly Tokenizer _tokenizer;
        private readonly ModelConfiguration _modelConfiguration;

        public InferenceService(ModelConfiguration modelConfiguration)
        {
            _modelConfiguration = modelConfiguration;
            _ogaHandle = new OgaHandle();
            var config = new Config(modelConfiguration.ModelDirectory);
            _model = new Model(config);
            _tokenizer = new Tokenizer(_model);
        }

        public IEnumerable<string> Prompt(string prompt, CancellationToken ct)
        {
            var formattedPrompt = _modelConfiguration.Template.FormatPrompt(prompt);

            using var sequences = _tokenizer.Encode(formattedPrompt);
            using var generatorParams = new GeneratorParams(_model);

            generatorParams.SetSearchOption("temperature", 0);
            generatorParams.SetSearchOption("top_k", 1);
            
            using var generator = new Generator(_model, generatorParams);
            generator.AppendTokenSequences(sequences);
            using var tokenizerStream = _tokenizer.CreateStream();

            while (!generator.IsDone() && !ct.IsCancellationRequested)
            {
                generator.GenerateNextToken();
                yield return tokenizerStream.Decode(generator.GetSequence(0)[^1]);
            }
        }

        public void Dispose()
        {
            _tokenizer.Dispose();
            _model.Dispose();
            _ogaHandle.Dispose();
        }
    }
}
