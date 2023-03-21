namespace Utils.TFSReader;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;

public class QueryExecutor
{
    private readonly Uri uri;
    private readonly string personalAccessToken;

    /// <summary>
    ///     Initializes a new instance of the <see cref="QueryExecutor" /> class.
    /// </summary>
    /// <param name="orgName">
    ///     An organization in Azure DevOps Services. If you don't have one, you can create one for free:
    ///     <see href="https://go.microsoft.com/fwlink/?LinkId=307137" />.
    /// </param>
    /// <param name="personalAccessToken">
    ///     A Personal Access Token, find out how to create one:
    ///     <see href="/azure/devops/organizations/accounts/use-personal-access-tokens-to-authenticate?view=azure-devops" />.
    /// </param>
    public QueryExecutor(string personalAccessToken)
    {
        this.uri = new Uri("http://ngt-tfs:8080/tfs/NeoGamesCollection/");
        this.personalAccessToken = personalAccessToken;
    }

    /// <summary>
    ///     Execute a WIQL (Work Item Query Language) query to return a list of open bugs.
    /// </summary>
    /// <param name="project">The name of your project within your organization.</param>
    /// <returns>A list of <see cref="WorkItem"/> objects representing all the open bugs.</returns>
    public IList<WorkItem> QueryOpenBugs(string project)
    {
        VssCredentials credentials = CreateCredentials();

        // create a wiql object and build our query
        var wiql = new Wiql()
        {
            // NOTE: Even if other columns are specified, only the ID & URL are available in the WorkItemReference
            Query = "Select [Id] " +
                    "From WorkItems " +
                    "Where [System.TeamProject] = '" + project + "' " +
                    "AND [System.WorkItemType] <> '' AND [System.AssignedTo] = 'Artem Shcherban' AND [System.State] <> 'Closed' AND [System.State] <> 'Resolved' AND [System.State] <> 'Removed'" +
                    "Order By [State] Asc, [Changed Date] Desc",
        };


        // create instance of work item tracking http client
        using (var httpClient = new WorkItemTrackingHttpClient(this.uri, credentials))
        {
            // execute the query to get the list of work items in the results
            var result = httpClient.QueryByWiqlAsync(wiql);

            var ids = result.Result.WorkItems.Select(item => item.Id).ToArray();

            // some error handling
            if (ids.Length == 0)
            {
                return Array.Empty<WorkItem>();
            }

            // build a list of the fields we want to see
            var fields = new[] { "System.Id", "System.WorkItemType", "System.Title", "System.State", "System.AssignedTo",
                 "Microsoft.VSTS.Scheduling.OriginalEstimate", "Microsoft.VSTS.Scheduling.RemainingWork", "Microsoft.VSTS.Scheduling.CompletedWork",
                 "Microsoft.VSTS.Scheduling.StartDate", "Microsoft.VSTS.Scheduling.FinishDate" };

            // get work items for the ids found in query
            return httpClient.GetWorkItemsAsync(ids, fields).Result;
        }


    }


    public IList<WorkItem> QueryFeatures(string project)
    {
        VssCredentials credentials = CreateCredentials();

        var wiql = new Wiql()
        {
            // NOTE: Even if other columns are specified, only the ID & URL are available in the WorkItemReference
            Query = "Select [Id] " +
                    "From WorkItems " +
                    "Where [System.TeamProject] = '" + project + "' " +
                    "AND [System.WorkItemType] = 'Feature' AND  [System.State] <> 'Closed' AND  [System.State] <> 'Removed' and [NG.Release] = '2023-05' and [System.AreaPath] under 'NeoAppAgile\\Lotteries'" +
                    "Order By [State] Asc  ASOF '" + DateTime.Today.ToString("MM-dd-yyyy") + "'",
        };


        // create instance of work item tracking http client
        using (var httpClient = new WorkItemTrackingHttpClient(this.uri, credentials))
        {
            // execute the query to get the list of work items in the results
            var result = httpClient.QueryByWiqlAsync(wiql);

            var ids = result.Result.WorkItems.Select(item => item.Id).ToArray();

            // some error handling
            if (ids.Length == 0)
            {
                return Array.Empty<WorkItem>();
            }

            // build a list of the fields we want to see
            var fields = new[] { "System.Id",  "System.Title", "System.State", "System.AssignedTo", "System.Tags", "NG.QAEstimation","NG.NetEstimation","NG.DBAEstimation","NG.FrontendEstimation", "NG.QAAutomationEstimation" };


            // get work items for the ids found in query
            //return httpClient.GetWorkItemsAsync(ids, fields).Result;
            return httpClient.GetWorkItemsAsync(ids, null, null,  WorkItemExpand.Relations).Result;
        }


    }

    private static VssCredentials CreateCredentials()
    {
        NetworkCredential networkCredential = new NetworkCredential("AlexeyD", "123456789$IT");
        Microsoft.VisualStudio.Services.Common.WindowsCredential winCred = new Microsoft.VisualStudio.Services.Common.WindowsCredential(networkCredential);
        VssCredentials credentials = new VssCredentials(winCred);
        return credentials;
    }


   

    /// <summary>
    ///     Execute a WIQL (Work Item Query Language) query to print a list of open bugs.
    /// </summary>
    /// <param name="project">The name of your project within your organization.</param>
    /// <returns>An async task.</returns>
    public void PrintOpenBugsAsync(string project)
    {
        var workItems = this.QueryOpenBugs(project);

        Console.WriteLine($"Query Results: {workItems.Count} items found");

        // loop though work items and write to console
        foreach (var workItem in workItems)
        {
            Console.WriteLine(
                "{0}\t{1}\t{2}",
                workItem.Id,
                workItem.Fields["System.Title"],
                workItem.Fields["System.State"]);
        }
    }
}