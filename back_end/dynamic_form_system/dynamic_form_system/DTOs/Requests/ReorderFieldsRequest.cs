namespace dynamic_form_system.DTOs.Requests
{
    public class ReorderFieldsRequest
    {
        public List<FieldOrderItem> Fields { get; set; }
        public class FieldOrderItem
        {
            public Guid Id { get; set; }
            public int DisplayOrder { get; set; }
        }
    }
}
