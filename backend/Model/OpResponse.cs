namespace backend.Model
{
    public class OpResponse<T> {
        public int Status { get; set; }
        public string Message { get; set; } = "";
        public string[] Errors { get; set; } = Array.Empty<string>();
        public T? Data { get; set; }

        public bool Ok() {
            return Status >= 200 && Status < 300;
        }
    }
}
