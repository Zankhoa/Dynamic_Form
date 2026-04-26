namespace dynamic_form_system.DTOs.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public Dictionary<string, string[]>? Errors { get; set; } // when error can response to use
    }
}
