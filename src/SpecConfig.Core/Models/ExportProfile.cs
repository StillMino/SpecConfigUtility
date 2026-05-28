using System.Collections.Generic;
namespace SpecConfig.Core.Models
{
    public class ExportProfile { public List<TableProfile> Tables { get; set; } = new(); }
    public class TableProfile { public string Caption { get; set; } = ""; public List<FieldProfile> Fields { get; set; } = new(); }
    public class FieldProfile { public string Caption { get; set; } = ""; public string Data { get; set; } = ""; public bool Visible { get; set; } = true; }
    public class ExtendedParameter { public string Name { get; set; } = ""; public string Value { get; set; } = "0"; }
    public class SpecifierProfile { public string ProfileName { get; set; } = ""; public string LinkedExportProfile { get; set; } = ""; }
}
