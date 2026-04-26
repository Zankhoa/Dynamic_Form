using System;
using System.Collections.Generic;

namespace dynamic_form_system.Data;

public partial class Form
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int DisplayOrder { get; set; }

    public string Status { get; set; } = null!;

    public Guid? UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<FormField> FormFields { get; set; } = new List<FormField>();

    public virtual ICollection<Submission> Submissions { get; set; } = new List<Submission>();

    public virtual User? User { get; set; }
}
