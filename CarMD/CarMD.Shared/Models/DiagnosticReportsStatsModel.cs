using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarMD.Shared.Models
{
    public class DiagnosticReportsStatsModel
    {
        public string ExternalSystemId { get; set; }
        public string MakesList { get; set; }
        public Nullable<DateTime> StartDateUTC { get; set; }
        public Nullable<DateTime> EndDateUTC { get; set; }
        public DataSourceRequest Request { get; set; }
    }

    public class DiagnosticReportsStatsGrid
    {
        public int ReportsTotal { get; set; }
        public string ReportsBasic { get; set; }
      
    }

    public class DiagnosticReportsStatsDDModel
    {
        public string Id { get; set; }
        public string Value { get; set; }
    }

    public class DiagnosticReportsStatsValues
    {
        public int ReportsTotal { get; set; }
        public int ReportsBasic { get; set; }
        public int ReportsAdvanced { get; set; }
        public int ReportsBasicCreatedWithFixNeeded { get; set; }
        public int ReportsAdvancedCreatedWithFixNeeded { get; set; }
        public int ReportsAdvancedCreatedWithFixNeededAndNowFixFound { get; set; }
        public int ReportsAdvancedWithRepairProcedure { get; set; }
        public int ReportsGreen { get; set; }
        public int ReportsYellow { get; set; }
        public int ReportsRed { get; set; }
        public int ReportsError { get; set; }
        public int ReportsTechRequested { get; set; }
    }

    public class DiagnosticReportsStatsGridHeader
    {
        public string DaysCount { get; set; }
        public string WeeksCount { get; set; }
        public string MonthsCount { get; set; }
        public string QuartersCount { get; set; }
        public string YearsCount { get; set; }
        public string Description { get; set; }

    }

    public class DiagnosticReportsStatsCaculatedValues
    {  
        public string DescriptionList { get; set; }
        public string DaysList { get; set; }
        public string WeeklyList { get; set; }
        public string MonthlyList { get; set; }
        public string QuarterlyList { get; set; }
        public string YearlyList { get; set; }
        public string TotalList { get; set; }
        
    }
}
