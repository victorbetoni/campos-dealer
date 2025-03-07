namespace backend.Model
{
    public class OpResponse<T> {
        private int status { get; set; }
        private string message { get; set; }
        private T? body { get; set; }

        public OpResponse(int status, string message, T? body)
        {
            this.status = status;
            this.message = message;
            this.body = body;
        }
    }
}
