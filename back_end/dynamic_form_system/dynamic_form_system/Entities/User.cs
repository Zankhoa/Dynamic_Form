using System;
using System.Collections.Generic;

namespace dynamic_form_system.Data;

public partial class User
{
    public Guid Id { get; set; }

    public string? Roles { get; set; }

    public virtual ICollection<Form> Forms { get; set; } = new List<Form>();

    public virtual ICollection<Submission> Submissions { get; set; } = new List<Submission>();
}
