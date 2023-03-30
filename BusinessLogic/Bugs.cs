namespace TFSViewer.BusinessLogic;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using TFSViewer.Utils;

public class Bugs
{

   
    public IList<WorkItem> QueryOpenBugs(string? assignedTo = null, DateTime? date = null)
    {
        DateTime currentDate = DateTime.Today;
        if (date.HasValue)
        {
            currentDate = date.Value;
        }

        string query = "Select [Id] " +
                    "From WorkItems " +
                    "Where [System.TeamProject] = '" + Config.Project + "' " +
                    "AND [System.WorkItemType] <> ''  AND [System.State] <> 'Closed' AND [System.State] <> 'Resolved' AND [System.State] <> 'Removed'" + (!string.IsNullOrWhiteSpace(assignedTo) ? " AND [System.AssignedTo] = '" + assignedTo + "' " : " ") +
                    "Order By [State] Asc, [Changed Date] Desc ASOF '" + currentDate.ToString("MM-dd-yyyy") + "'";
        return QueryExecutor.ExecuteQuery(query);
    }

}