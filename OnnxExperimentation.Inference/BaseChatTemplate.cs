namespace OnnxExperimentation.Inference
{
    public abstract record BaseChatTemplate
    {
        public abstract string Template { get;}

        public string FormatPrompt(string prompt)
        {
            return string.Format(Template, prompt);
        }
    }
}
