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
    public string date { get; set; }

    [BindProperty(SupportsGet = true)]
    public string team { get; set; }

    public List<string> Releases = new List<string>();

    public List<string> Teams = new List<string>();

    public string CurrentRelease { get; set; } = "";


    public FHistoryModel(IConfiguration configuration)
    {
        info = new FeaturesInfo(new List<WorkItem>());
    }

    public void OnGet()
    {

        CurrentRelease = BusinessLogic.Releases.GetCurrentRelease();
        DateTime dt = BusinessLogic.Releases.GetDateOfCurrentRelease();
        Releases = BusinessLogic.Releases.GetReleases(dt.AddMonths(-5), dt);

        Teams = BusinessLogic.Teams.QueryTeams().ToList();

        var workItems = Features.QueryFeatures(CurrentRelease, ParseDate(date));
        info = new FeaturesInfo(workItems.ToList());
    }

    private DateTime ParseDate(string date)
    {
        DateTime result = DateTime.Today;
        if (!string.IsNullOrWhiteSpace(date))
        {
            DateTime.TryParseExact(date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out result);
        }
        return result;
    }

                       
}
