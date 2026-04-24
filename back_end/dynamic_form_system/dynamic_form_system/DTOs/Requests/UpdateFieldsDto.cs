using System.ComponentModel.DataAnnotations;

namespace dynamic_form_system.DTOs.Requests
{
    public class UpdateFieldDto
    {

        [Required(ErrorMessage = "Name không được để trống.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Label không được để trống.")]
        public string Label { get; set; }

        [Required]
        public string FieldType { get; set; }

        public bool IsRequired { get; set; }
        public string Configuration { get; set; } = "{}";
    }
}
