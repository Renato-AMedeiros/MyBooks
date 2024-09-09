namespace my_library_cosmos_db.Middlewares
{
    public class ErrorMiddlewareResponse
    {
        public ErrorMiddlewareResponse()
        {
            TraceId = Guid.NewGuid();
            Errors = new List<ErrorDetailsResponse>();
        }

        public ErrorMiddlewareResponse(string logref, string message)
        {
            TraceId = Guid.NewGuid();
            Errors = new List<ErrorDetailsResponse>();
            AddError(logref, message);
        }

        public Guid TraceId { get; private set; }
        public List<ErrorDetailsResponse> Errors { get; private set; }

        public class ErrorDetailsResponse
        {
            public ErrorDetailsResponse(string logref, string message)
            {
                Logref = logref;
                Message = message;
            }

            public string Logref { get; private set; }
            public string Message { get; private set; }
        }

        public void AddError(string logref, string message)
        {
            Errors.Add(new ErrorDetailsResponse(logref, message));
        }
    }
}