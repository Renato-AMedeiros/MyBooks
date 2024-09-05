namespace my_library_cosmos_db.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }

    }

    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message) { }

    }
}
