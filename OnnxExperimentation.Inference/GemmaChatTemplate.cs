namespace OnnxExperimentation.Inference
{
    public record GemmaChatTemplate : BaseChatTemplate
    {
        public override string Template { get; }

        public GemmaChatTemplate()
        {
            Template = "<start_of_turn>user\n{0}<end_of_turn>\n<start_of_turn>model\n";
        }
    }
}
