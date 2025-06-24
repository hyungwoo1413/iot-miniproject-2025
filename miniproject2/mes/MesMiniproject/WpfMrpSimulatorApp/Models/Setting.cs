using System;
using System.Collections.Generic;

namespace WpfMrpSimulatorApp.Models;

public partial class Setting
{
    public string BasicCode { get; set; } = null!;

    public string CodeName { get; set; } = null!;

    public string? CodeDesc { get; set; }

    public DateTime? RegDt { get; set; }

    public DateTime? ModDt { get; set; }
}
