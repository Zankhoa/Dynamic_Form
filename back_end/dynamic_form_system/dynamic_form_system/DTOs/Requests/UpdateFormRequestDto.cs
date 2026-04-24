using System.ComponentModel.DataAnnotations;

namespace dynamic_form_system.DTOs.Requests
{
    public class UpdateFormRequestDto
    {
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        [MaxLength(255)]
        public string Title { get; set; }

        public string Description { get; set; }

        public int DisplayOrder { get; set; }

        public string Status { get; set; }
    }
}
