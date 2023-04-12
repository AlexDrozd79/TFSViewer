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


    public static IList<WorkItem> QueryOpenBugs(string assignedTo = null, DateTime date = default)
    {

        string query = "Select [Id] " +
                    "From WorkItems " +
                    "Where [System.TeamProject] = '" + Config.Project + "' " +
                    "AND [System.WorkItemType] <> ''  AND [System.State] <> 'Closed' AND [System.State] <> 'Resolved' AND [System.State] <> 'Removed'" + (!string.IsNullOrWhiteSpace(assignedTo) ? " AND [System.AssignedTo] = '" + assignedTo + "' " : " ") +
                    "Order By [State] Asc, [Changed Date] Desc ASOF '" + date.ToString("MM-dd-yyyy") + "'";
        return QueryExecutor.ExecuteQuery(query);
    }

    public static IList<WorkItem> GetUATBugs(string areaPath = null, DateTime date = default)
    {
        DateTime currentDate = DateTime.Today;
        if (date != default)
        {
            currentDate = date;
        }

        if (string.IsNullOrWhiteSpace(areaPath))
        {
            areaPath = "NeoAppAgile\\Lotteries";
        }

         string query = "Select [Id] " +
                    "From WorkItems " +
                    "Where [System.TeamProject] = '" + Config.Project + "' " +
                    "AND [System.WorkItemType] = 'Bug'  AND [System.State] <> 'Closed' AND [System.AreaPath] under '" + areaPath + "' " + 
                    "AND ([Microsoft.VSTS.CMMI.FoundInEnvironment] = 'UAT' OR [System.Title] contains words 'UAT') " + 
                    "AND [System.AssignedTo] <> 'Anastacia Benshalom <CORP\\AnastaciaB>' AND [System.AssignedTo] <> 'Galit Morash <CORP\\GalitM>' AND [System.AssignedTo] <> 'Tamir Bachrach <CORP\\TamirB>' " +
                    "Order By [ID] Asc  ASOF '" + currentDate.ToString("MM-dd-yyyy") + "'";
        
        return QueryExecutor.ExecuteQuery(query);
    }

     public static IList<WorkItem> GetProductionBugs(string areaPath = null, DateTime date = default)
    {
        DateTime currentDate = DateTime.Today;
        if (date != default)
        {
            currentDate = date;
        }

        if (string.IsNullOrWhiteSpace(areaPath))
        {
            areaPath = "NeoAppAgile\\Lotteries";
        }

         string query = "Select [Id] " +
                    "From WorkItems " +
                    "Where [System.TeamProject] = '" + Config.Project + "' " +
                    "AND [System.WorkItemType] = 'Bug'  AND [System.State] <> 'Closed' AND [System.AreaPath] under '" + areaPath + "' " + 
                    "AND ([Microsoft.VSTS.CMMI.FoundInEnvironment] = 'Production' OR [System.Title] contains words 'prod') " + 
                    "AND [System.AssignedTo] <> 'Anastacia Benshalom <CORP\\AnastaciaB>' AND [System.AssignedTo] <> 'Galit Morash <CORP\\GalitM>' AND [System.AssignedTo] <> 'Tamir Bachrach <CORP\\TamirB>' " +
                    "Order By [ID] Asc  ASOF '" + currentDate.ToString("MM-dd-yyyy") + "'"; 
        
        return QueryExecutor.ExecuteQuery(query);
    }

}