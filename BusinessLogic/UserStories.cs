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

public static class UserStories
{
    public static IList<WorkItem> QueryUserStories(string release,  DateTime date, string areaPath)
    {

        string query = "Select [Id] " +
                    "From WorkItems " +
                    "Where [System.TeamProject] = '" + Config.Project + "' " +
                    "AND [System.WorkItemType] = 'User Story' AND  [System.State] <> 'Removed' and [NG.Release] = '" + release + "' and [System.AreaPath] under '" + areaPath + "'" +
                    "Order By [ID] Asc  ASOF '" + date.ToString("MM-dd-yyyy") + "'";

        return QueryExecutor.ExecuteQuery(query);

    }

}