namespace WebApi.Infrastructure.Exceptions
{
    public class CatalogAppExceptions
        : Exception
    {
        public CatalogAppExceptions()
        {

        }

        public CatalogAppExceptions(string message)
            : base(message)
        {

        }

        public CatalogAppExceptions(string message, string? details)
            : base($"{message} Details: {details}")
        {
        }

        public CatalogAppExceptions(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
