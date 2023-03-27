using LiftApp.Export.Enums;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.Export.Options
{
    public class OverviewExportOptions
    {
        public string ExcelTemplatePath { get; set; } = default!;
        public Dictionary<OverviewType, string> SheetNames { get; set; } = default!;
    }
}
