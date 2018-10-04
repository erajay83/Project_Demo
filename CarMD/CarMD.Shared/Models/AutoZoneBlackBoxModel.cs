using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarMD.Shared.Models
{
    public class AutoZoneBlackBoxModel
    {
        public Nullable<DateTime> StartTime { get; set; }
        public Nullable<DateTime> EndTime { get; set; }
        public DataSourceRequest Request { get; set; }
    }

    public class AutoZoneBlackBoxGrid
    {
        public string DiagnosticReportId { get; set; }
        public string DiagnosticReportResultId { get; set; }
        public string VehicleId { get; set; }
        public string ExternalSystemReportId { get; set; }
        public DateTime ReportDate { get; set; }
        public DateTime ReportTime { get; set; }
        public string ReportDateStr { get; set; }
        public string ReportTimeStr { get; set; }
        public Nullable<int> Year { get; set; }
        public string Email { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string EngineType { get; set; }
        public string TransmissionControlType { get; set; }
        public Nullable<int> ToolLEDStatus { get; set; }
        public string Description { get; set; }

    }
}
