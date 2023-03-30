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

public class Releases
{

    private readonly IConfiguration Configuration;
    public Releases(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public List<string> GetReleases(string project)
    {
        List<string> releases = new List<string>();
        string currentRelease = GetCurrentRelease(project);
        string[] parts = currentRelease.Split("-");
        DateTime currentDate = new DateTime(int.Parse(parts[0]), int.Parse(parts[1]), 1);
        for (DateTime dt = currentDate; dt <= currentDate.AddMonths(3); dt = dt.AddMonths(1))
        {
            releases.Add(dt.Year + "-" + (dt.Month.ToString().Length == 1 ? "0" + dt.Month: dt.Month) );
        }

        return releases;
    }

    public string GetCurrentRelease(string project)
    {
        string result = "";
        DateTime currentDate = DateTime.Today;
        
        string query = "Select [Id] " +
                    "From WorkItems " +
                    "Where [System.TeamProject] = '" + project + "' " +
                    "AND [System.WorkItemType] = 'Feature' AND  [System.State] <> 'Closed' AND  [System.State] <> 'Removed' and [System.AreaPath] under 'NeoAppAgile\\Lotteries' " +
                    "AND [System.IterationPath] UNDER  @currentIteration('[NeoAppAgile]\\Happy3Friends <id:33416386-7af4-4260-9888-83f99242a272>')";

        QueryExecutor queryExecutor = new QueryExecutor(Configuration);
        IList<WorkItem> items = queryExecutor.ExecuteQuery(query);
        if (items.Count > 0)
        {
            result = items[0].Fields["NG.Release"].ToString() ?? "";
        }

        return result;

    } 
}