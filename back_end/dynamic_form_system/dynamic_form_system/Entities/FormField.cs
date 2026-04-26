using System;
using System.Collections.Generic;

namespace dynamic_form_system.Data;

public partial class FormField
{
    public Guid Id { get; set; }

    public Guid FormId { get; set; }

    public string Name { get; set; } = null!;

    public string Label { get; set; } = null!;

    public string FieldType { get; set; } = null!;

    public int DisplayOrder { get; set; }

    public bool IsRequired { get; set; }

    public string Configuration { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Form Form { get; set; } = null!;
}
