namespace Technical.Test.Business.Untils
{
    public class CustomMessage<TClass>
    {
        public bool Success { get; set; }

        public TClass Data { get; set; }
    }
}
