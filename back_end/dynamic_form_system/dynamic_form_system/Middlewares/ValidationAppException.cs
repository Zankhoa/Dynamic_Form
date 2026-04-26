namespace dynamic_form_system.Middlewares
{
    public class ValidationAppException : Exception
    {
        //public object Errors { get; }

        //public ValidationAppException(string message, object errors = null) : base(message)
        //{
        //    Errors = errors;
        //}
        public Dictionary<string, string> Errors { get; }

        // 1. Constructor nhận vào một Dictionary (Để sửa cái lỗi đỏ trong hình của bạn)
        public ValidationAppException(Dictionary<string, string> errors)
            : base("Có lỗi xảy ra trong quá trình kiểm tra dữ liệu.")
        {
            Errors = errors;
        }

        // 2. Constructor nhận vào 1 lỗi duy nhất (Cực kỳ tiện lợi nếu chỉ có 1 field sai)
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
