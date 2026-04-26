namespace dynamic_form_system.Middlewares
{
    public class ValidationAppException : Exception
    {
        public object Errors { get; }

        public ValidationAppException(string message, object errors = null) : base(message)
        {
            Errors = errors;
        }
    }
}
