namespace Defra.TradeImportsDataApi.Api.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        private const string _message = "{0} with Id {1} not found";

        public EntityNotFoundException(string entityType, string entityId)
            : base(string.Format(_message, entityType, entityId)) { }

        public EntityNotFoundException(string entityType, string entityId, Exception? innerException)
            : base(string.Format(_message, entityType, entityId), innerException) { }
    }
}
