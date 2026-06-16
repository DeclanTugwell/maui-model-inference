namespace OnnxExperimentation.Inference
{
    public record PhiChatTemplate : BaseChatTemplate
    {
        public override string Template { get; }

        public PhiChatTemplate()
        {
            Template = "<|user|>{0}<|end|><|assistant|>";
        }
    }
}
