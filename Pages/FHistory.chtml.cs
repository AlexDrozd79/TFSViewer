using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Enteties;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace TFSViewer.Pages;

public class FHistoryModel : PageModel
{
    private readonly IConfiguration Configuration;
    public FeaturesInfo info;

    [BindProperty(SupportsGet = true)]
    public string? date { get; set; }


    public FHistoryModel(IConfiguration configuration)
    {
        Configuration = configuration;
        info = new FeaturesInfo(new List<WorkItem>());
    }

    public void OnGet()
    {
        BusinessLogic.Features features = new BusinessLogic.Features(Configuration);
        var workItems = features.QueryFeatures("NeoAppAgile", ParseDate(date));
        info = new FeaturesInfo(workItems.ToList());
    }

    private DateTime ParseDate(string? date)
    {
        DateTime result = DateTime.Today;
        if (!string.IsNullOrWhiteSpace(date))
        {
            DateTime.TryParseExact(date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out result);
        }
        return result;
    }
}
