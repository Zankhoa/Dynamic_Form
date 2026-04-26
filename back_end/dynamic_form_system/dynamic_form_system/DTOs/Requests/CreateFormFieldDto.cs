using System.ComponentModel.DataAnnotations;

namespace dynamic_form_system.DTOs.Requests
{
    public class CreateFormFieldDto
    {
        [Required]
        public string Name { get; set; } 

        [Required]
        public string Label { get; set; } 

        [Required]
        public string FieldType { get; set; } 

        public int DisplayOrder { get; set; }

        public bool IsRequired { get; set; }

        public string Configuration { get; set; } = "{}"; 
    }
}
