namespace TFSViewer.Utils;

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


    public static IList<WorkItem> ExecuteQuery(string WIQL, DateTime? asOff = null)
    {
        VssCredentials credentials = CreateCredentials();

        var wiql = new Wiql()
        {
            // NOTE: Even if other columns are specified, only the ID & URL are available in the WorkItemReference
            Query = WIQL,
        };


        // create instance of work item tracking http client
        using (var httpClient = new WorkItemTrackingHttpClient(new Uri(Config.URI ?? ""), credentials))
        {
            // execute the query to get the list of work items in the results
            var result = httpClient.QueryByWiqlAsync(wiql);
            

            var ids = result.Result.WorkItems.Select(item => item.Id).ToArray();

            // some error handling
            IList<WorkItem> items = Array.Empty<WorkItem>();
            if (ids.Length == 0)
            {
                return items;
            }

            try
            {
                if (ids.Length <= 200)
                {
                    items = httpClient.GetWorkItemsAsync(ids, null, asOff).Result;
                }
                else
                {
                    List<WorkItem> lst = new List<WorkItem>();
                    while (ids.Length > 0)
                    {
                        lst.AddRange(httpClient.GetWorkItemsAsync(ids.Take(200), null, asOff).Result);
                        ids = ids.Skip(200).ToArray();
                    }
                    items = lst;
                }

            }
            catch (System.AggregateException ex)
            {
                items = HandleAggregateException(ids, httpClient, asOff, ex);
            }

            return items;
        }


    }



    public static IList<WorkItem> ExecuteQueryForLinkItems(string WIQL, List<string> fields)
    {
        VssCredentials credentials = CreateCredentials();

        var wiql = new Wiql()
        {
            // NOTE: Even if other columns are specified, only the ID & URL are available in the WorkItemReference
            Query = WIQL,
        };


        // create instance of work item tracking http client
        using (var httpClient = new WorkItemTrackingHttpClient(new Uri(Config.URI ?? ""), credentials))
        {
            // execute the query to get the list of work items in the results
            var result = httpClient.QueryByWiqlAsync(wiql);

            var ids = result.Result.WorkItemRelations.Select(item => item.Target.Id).ToArray();
            

            // some error handling
            IList<WorkItem> items = Array.Empty<WorkItem>();
            if (ids.Length == 0)
            {
                return items;
            }

            if (ids.Length <= 200)
            {
                items = httpClient.GetWorkItemsAsync(ids, fields).Result;
            }
            else
            {
                List<WorkItem> lst = new List<WorkItem>();
                while (ids.Length > 0)
                {
                    lst.AddRange(httpClient.GetWorkItemsAsync(ids.Take(200), fields).Result);
                    ids = ids.Skip(200).ToArray();
                }
                items = lst;
            }

            return items;
        }


    }

    private static IList<WorkItem> HandleAggregateException(int[] ids, WorkItemTrackingHttpClient httpClient, DateTime? asOff, AggregateException ex)
    {
        IList<WorkItem> items = new List<WorkItem>();
        bool hasResult = false;
        int count = 0;

        do
        {
            ids = RemoveCorruptedID(ex, ids);

            try
            {
                items = httpClient.GetWorkItemsAsync(ids, null, asOff).Result;
                hasResult = true;
            }
            catch (System.AggregateException ex2)
            {
                ids = RemoveCorruptedID(ex2, ids);
                hasResult = false;
                count++;
            }
        }
        while (!hasResult && count <= 3);

        return items;
    }

    private static int[] RemoveCorruptedID(AggregateException ex, int[] ids)
    {
        if (ex.Message.Contains("TF401232"))
        {
            string itemID = ex.Message.Split("Work item")[1].Split("does not exist")[0].Trim();
            ids = ids.Where(i => i != int.Parse(itemID)).ToArray();
        }
        else
        {
            throw ex;
        }
        return ids;
    }


    private static VssCredentials CreateCredentials()
    {

        string personalToken = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(Config.Token));
        return new VssBasicCredential(string.Empty, personalToken);
    }


}