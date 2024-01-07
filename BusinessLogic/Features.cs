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
    public static IList<WorkItem> QueryFeatures(string release, DateTime date, string areaPath = "")
    {

        DateTime currentDate = date;

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

    public static IList<WorkItem> QueryFeaturesRecursive(string release, DateTime date, string areaPath = "")
    {

        DateTime currentDate = date;

        if (string.IsNullOrWhiteSpace(areaPath))
        {
            areaPath = "NeoAppAgile\\Lotteries";
        }

        string query = "Select [Id] " +
                    "From WorkItemLinks " +
                    "Where [Source].[System.TeamProject] = '" + Config.Project + "' " +
                    "AND [Source].[System.WorkItemType] = 'Feature' AND [Source].[System.State] <> 'Removed' AND [Source].[System.State] <> 'Closed' " +
                    "AND [Source].[NG.Release] = '" + release + "' AND [Source].[System.AreaPath] under '" + areaPath + "' " +
                    "AND ([System.Links.LinkType] = 'System.LinkTypes.Hierarchy-Forward') AND ([Target].[System.TeamProject] = '" + Config.Project + "' AND [Target].[System.WorkItemType] <> '') mode(Recursive)";

        return QueryExecutor.ExecuteQueryForLinkItems(query, new List<string> {"System.Id", "System.WorkItemType", "System.State", "System.AssignedTo",
                    "System.Title", "NG.FrontendEstimation", "NG.QAEstimation", "NG.NetEstimation", "NG.QAAutomationEstimation", "NG.DBAEstimation", "Microsoft.VSTS.Scheduling.OriginalEstimate",
                    "System.Parent"});

    }

   
}