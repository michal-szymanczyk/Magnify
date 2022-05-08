namespace MagnifyLogistics.Common
{
    public class ValidationException : Exception
    {
        // very general validation exception - for simplicity
        // in overall it could have list of validation errors etc.
        public ValidationException(string message) 
            : base(message) { }
    }
}
