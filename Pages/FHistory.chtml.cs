using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Enteties;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using TFSViewer.BusinessLogic;

namespace TFSViewer.Pages;

public class FHistoryModel : PageModel
{
    public FeaturesInfo info;

    [BindProperty(SupportsGet = true)]
    public string? date { get; set; }


    public FHistoryModel(IConfiguration configuration)
    {
        info = new FeaturesInfo(new List<WorkItem>());
    }

    public void OnGet()
    {
        string currentRelease = Releases.GetCurrentRelease();

        var workItems = Features.QueryFeatures(currentRelease, ParseDate(date));
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
