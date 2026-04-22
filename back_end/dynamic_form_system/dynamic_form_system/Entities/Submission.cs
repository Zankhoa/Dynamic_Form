using System;
using System.Collections.Generic;

namespace dynamic_form_system.Entities;

public partial class Submission
{
    public Guid Id { get; set; }

    public Guid FormId { get; set; }

    public Guid? UserId { get; set; }

    public DateTime? SubmittedAt { get; set; }

    public string Data { get; set; } = null!;

    public string? ExtractedCity { get; set; }

    public virtual Form Form { get; set; } = null!;
}
