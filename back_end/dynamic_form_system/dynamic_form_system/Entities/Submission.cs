using System;
using System.Collections.Generic;

namespace dynamic_form_system.Data;

public partial class Submission
{
    public Guid Id { get; set; }

    public Guid FormId { get; set; }

    public Guid? UserId { get; set; }

    public DateTime SubmittedAt { get; set; }

    public string Data { get; set; } = null!;

    public virtual Form Form { get; set; } = null!;

    public virtual User? User { get; set; }
}
