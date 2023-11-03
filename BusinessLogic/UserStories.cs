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
    public static IList<WorkItem> QueryUserStories(string release, DateTime date, string areaPath, bool fullReverse)
    {

        string query = "Select [Id] " +
                    "From WorkItems " +
                    "Where [System.TeamProject] = '" + Config.Project + "' " +
                    "AND [System.WorkItemType] = 'User Story' AND  [System.State] <> 'Removed' and [NG.Release] = '" + release + "' and [System.AreaPath] under '" + areaPath + "'" +
                    "Order By [ID] Asc  ASOF '" + date.ToString("MM-dd-yyyy HH:mm:ss") + "'";

        return QueryExecutor.ExecuteQuery(query, fullReverse ? date : null);

    }


    public static IList<WorkItem> QueryUserStoriesChildItems(string release, DateTime date, string areaPath)
    {
        string query = " SELECT [Id] " +
             "FROM WorkItemLinks " +
             "WHERE  [Source].[System.TeamProject] = '" + Config.Project + "' " +
             "AND [Source].[System.WorkItemType] = 'User Story' AND System.Links.LinkType = 'System.LinkTypes.Hierarchy-Forward'  AND  [Source].[System.State] <> 'Removed' " +
             "AND [Source].[NG.Release] = '" + release + "' and [Source].[System.AreaPath] under '" + areaPath + "' " +
             "ORDER BY [System.Id] mode(MustContain)";

        IList<WorkItem> items = QueryExecutor.ExecuteQueryForLinkItems(query, new List<string>() { "[System.Parent]", "[System.WorkItemType]", "[Microsoft.VSTS.Common.ClosedDate]" });

       
        return items;


    }

}