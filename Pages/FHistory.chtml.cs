using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Utils.TFSReader;
using Enteties;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace TFSViewer.Pages;

public class FHistoryModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    public FeaturesInfo info;

    [BindProperty(SupportsGet = true)]
    public string? date { get; set; }


    public FHistoryModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
        info = new FeaturesInfo(new List<WorkItem>());
    }

    public void OnGet()
    {
        QueryExecutor queryExecutor = new QueryExecutor("eifbign7v2yqqtcbbairsvdrwvqd4f7v3brftrtl53pocxoqnt2a");
        var workItems = queryExecutor.QueryFeatures("NeoAppAgile", ParseDate(date));
        info = new FeaturesInfo(workItems.ToList());
    }

    private DateTime ParseDate(string? date)
    {
        DateTime  result = DateTime.Today;
        if (!string.IsNullOrWhiteSpace(date))
        {
            DateTime.TryParseExact(date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out result);
        }
        return result;
    }
}
