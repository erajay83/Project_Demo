using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarMD.Shared.Models
{
    public class DiagnosticReportTrendsModel
    {
        public string SystemID { get; set; }
        public Nullable<DateTime> StartDateTime { get; set; }
        public Nullable<DateTime> EndDateTime { get; set; }
        public string GroupBy { get; set; }
        public DataSourceRequest Request { get; set; }
    }

    public class DiagnosticReportTrendsGrid
    {
        public string DiagnosticReportId { get; set; }
        public string ReportDate { get; set; }
        public int TotalReports { get; set; }
        public int TotalFixReports { get; set; }
        public int TotalFixProvidedLaterReports { get; set; }
        public int TotalNoFixReports { get; set; }
    }

    public class TrendDropDowdModel
    {
        public string Id { get; set; }
        public string Value { get; set; }
    }
}
