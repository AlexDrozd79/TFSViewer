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

public static class Features
{
    public static IList<WorkItem> QueryFeatures(string release,  DateTime? date = null, string? areaPath = "")
    {

        DateTime currentDate = DateTime.Today;
        if (date.HasValue)
        {
            currentDate = date.Value;
        }

        if (string.IsNullOrWhiteSpace(areaPath))
        {
            areaPath = "NeoAppAgile\\Lotteries";
        }

        string query = "Select [Id] " +
                    "From WorkItems " +
                    "Where [System.TeamProject] = '" + Config.Project + "' " +
                    "AND [System.WorkItemType] = 'Feature' AND  [System.State] <> 'Closed' AND  [System.State] <> 'Removed' and [NG.Release] = '" + release + "' and [System.AreaPath] under '" + areaPath + "'" +
                    "Order By [ID] Asc  ASOF '" + currentDate.ToString("MM-dd-yyyy") + "'";

        return QueryExecutor.ExecuteQuery(query);

    }

}