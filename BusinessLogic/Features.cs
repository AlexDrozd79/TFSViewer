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

public class Features
{
    private readonly IConfiguration Configuration;
    public Features(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IList<WorkItem> QueryFeatures(string project, DateTime? date = null)
    {

        DateTime currentDate = DateTime.Today;
        if (date.HasValue)
        {
            currentDate = date.Value;
        }

        string query = "Select [Id] " +
                    "From WorkItems " +
                    "Where [System.TeamProject] = '" + project + "' " +
                    "AND [System.WorkItemType] = 'Feature' AND  [System.State] <> 'Closed' AND  [System.State] <> 'Removed' and [NG.Release] = '2023-05' and [System.AreaPath] under 'NeoAppAgile\\Lotteries'" +
                    "Order By [ID] Asc  ASOF '" + currentDate.ToString("MM-dd-yyyy") + "'";

        QueryExecutor queryExecutor = new QueryExecutor(Configuration);
        return queryExecutor.ExecuteQuery(query);

    }

}