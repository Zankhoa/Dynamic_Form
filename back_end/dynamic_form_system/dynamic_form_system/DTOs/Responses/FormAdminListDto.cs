namespace dynamic_form_system.DTOs.Responses
{
    public class FormAdminListDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int DisplayOrder { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
