namespace OnnxExperimentation.Maui;

public static class ModelLoader
{
    public static async Task<string> PrepareModelAsync()
    {
        var targetDir = Path.Combine(FileSystem.AppDataDirectory, "models", "gemma-3-1b");

        if (Directory.Exists(targetDir))
            return targetDir;

        Directory.CreateDirectory(targetDir);

        var files = new[]
        {
            "config.json",
            "genai_config.json",
            "generation_config.json",
            "model_q4f16.onnx",
            "model_q4f16.onnx_data",
            "special_tokens_map.json",
            "tokenizer.json",
            "tokenizer_config.json"
        };

        foreach (var file in files)
        {
            await using var stream = await FileSystem.OpenAppPackageFileAsync($"Models/gemma-3-1b/{file}");
            var targetPath = Path.Combine(targetDir, file);

            await using var outStream = File.Create(targetPath);
            await stream.CopyToAsync(outStream);
        }

        return targetDir;
    }
}