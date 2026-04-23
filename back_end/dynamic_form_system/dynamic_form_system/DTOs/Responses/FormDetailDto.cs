namespace dynamic_form_system.DTOs.Responses
{
    public class FormDetailDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Chứa danh sách các trường dữ liệu
        public List<FormFieldDto> Fields { get; set; } = new();
    }
}
