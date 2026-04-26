namespace dynamic_form_system.DTOs.Responses
{
    public class SubmissionHistoryDto
    {
        public Guid Id { get; set; }
        public Guid FormId { get; set; }
        public string FormTitle { get; set; }
        public DateTime SubmittedAt { get; set; }
        public string Data { get; set; }
    }
}
