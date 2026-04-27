namespace dynamic_form_system.Middlewares
{
    public class ValidationAppException : Exception
    {
        public Dictionary<string, string> Errors { get; }

        // 1. Constructor get a Dictionary 
        public ValidationAppException(Dictionary<string, string> errors)
            : base("Có lỗi xảy ra trong quá trình kiểm tra dữ liệu.")
        {
            Errors = errors;
        }

        // 2. Constructor get just a error 
        public ValidationAppException(string field, string message)
            : base("Có lỗi xảy ra trong quá trình kiểm tra dữ liệu.")
        {
            Errors = new Dictionary<string, string>
            {
                { field, message }
            };
        }
    }
}
