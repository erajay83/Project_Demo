using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarMD.Shared.Models
{
    public class AdminUserStatisticModel
    {
        public string User { get; set; }
        public int ReportsClosedNewFixAdded { get; set; }
        public int ReportsClosedExistingFixSelected { get; set; }
        public int ReportsClosedRejected { get; set; }
        public int NumOfFixesFromFixReport { get; set; }
        public int NumOfDirectFixes { get; set; }
        public int NumOfTotalReport { get; set; }

    }

    public class StatisticsFilter
    {
        public string UserID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DataSourceRequest Request { get; set; }
    }
}
