using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Utils.TFSReader;
using Enteties;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace TFSViewer.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    public FeaturesInfo info;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
        info = new FeaturesInfo(new List<WorkItem>());
    }

    public void OnGet()
    {
        QueryExecutor queryExecutor = new QueryExecutor("eifbign7v2yqqtcbbairsvdrwvqd4f7v3brftrtl53pocxoqnt2a");
        var workItems = queryExecutor.QueryFeatures("NeoAppAgile");
        info = new FeaturesInfo( workItems.ToList());
    }
}
