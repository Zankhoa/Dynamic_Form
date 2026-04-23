using System.ComponentModel.DataAnnotations;

namespace dynamic_form_system.DTOs.Requests
{
    public class CreateFormRequestDto
    {
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        [MaxLength(255)]
        public string Title { get; set; }

        public string Description { get; set; }

        public int DisplayOrder { get; set; }

        public string Status { get; set; } // 0: Draft, 1: Active

        // Danh sách các trường (Fields) đi kèm Form
        [Required]
        [MinLength(1, ErrorMessage = "Form phải có ít nhất 1 trường dữ liệu")]
        public List<CreateFormFieldDto> Fields { get; set; } = new();
    }
}
