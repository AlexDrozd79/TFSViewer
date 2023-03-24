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
    private readonly IConfiguration Configuration;

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
    public QueryExecutor(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IList<WorkItem> ExecuteQuery(string WIQL)
    {
        VssCredentials credentials = CreateCredentials();

        var wiql = new Wiql()
        {
            // NOTE: Even if other columns are specified, only the ID & URL are available in the WorkItemReference
            Query = WIQL,
        };


        // create instance of work item tracking http client
        using (var httpClient = new WorkItemTrackingHttpClient(new Uri(Configuration.GetValue<string>("URI")??""), credentials))
        {
            // execute the query to get the list of work items in the results
            var result = httpClient.QueryByWiqlAsync(wiql);

            var ids = result.Result.WorkItems.Select(item => item.Id).ToArray();

            // some error handling
            if (ids.Length == 0)
            {
                return Array.Empty<WorkItem>();
            }

            return httpClient.GetWorkItemsAsync(ids, null, null,  WorkItemExpand.Relations).Result;
        }


    }

    private VssCredentials CreateCredentials()
    {
        NetworkCredential networkCredential = new NetworkCredential(Configuration.GetValue<string>("user"), Configuration.GetValue<string>("password"));
        Microsoft.VisualStudio.Services.Common.WindowsCredential winCred = new Microsoft.VisualStudio.Services.Common.WindowsCredential(networkCredential);
        VssCredentials credentials = new VssCredentials(winCred);
        return credentials;
    }


}