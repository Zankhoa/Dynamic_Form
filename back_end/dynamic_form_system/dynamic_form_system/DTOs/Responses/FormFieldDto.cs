namespace dynamic_form_system.DTOs.Responses
{
    public class FormFieldDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string FieldType { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsRequired { get; set; }
        public string Configuration { get; set; }
    }
}
