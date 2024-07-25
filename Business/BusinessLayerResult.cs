namespace Business
{
    public class BusinessLayerResult<T> where T : class
    {
        public bool result { get; set; } = true;
        public T? Data { get; set; }
        public List<T>? DataList { get; set; }
        public List<string>? Errors { get; set; }
        public BusinessLayerResult() => Errors = new List<string>();
        public void AddError(string message)
        {
            result = false;
            Errors?.Add(message);
        }
    }
}
