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

public class Tasks
{

    public static IList<WorkItem> QueryTasks(string release, DateTime date, string areaPath)
    {

        string[] iterations = Releases.GetReleases(5, 0).Where(r => r.ReleaseName == release).SelectMany(r => r.Iterations).ToArray();

        string itrExpr = " and (";
        for (int i = 0; i < iterations.Length; i++)
        {
            itrExpr += "[System.IterationPath] = '" + iterations[i] + "'";

            if (i < iterations.Length - 1)
            {
                itrExpr += " or ";
            }   
        }
        itrExpr += ") ";

        string query = "Select [Id] " +
                    "From WorkItems " +
                    "Where [System.TeamProject] = '" + Config.Project + "' " +
                    "AND [System.WorkItemType] = 'Task' AND  [System.State] <> 'Removed' " + itrExpr +
                    "AND [System.AreaPath] under '" + areaPath + "'" +
                    "Order By [ID] Asc  ASOF '" + date.ToString("MM-dd-yyyy") + "'";

        return QueryExecutor.ExecuteQuery(query);

    }


}