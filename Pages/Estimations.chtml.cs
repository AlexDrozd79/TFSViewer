using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Enteties;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using TFSViewer.BusinessLogic;

namespace TFSViewer.Pages;

public class EstimationsModel : PageModel
{
    public FeaturesInfo info;

    [BindProperty(SupportsGet = true)]
    public string date { get; set; }

    [BindProperty(SupportsGet = true)]
    public string team { get; set; }

    public List<string> Releases = new List<string>();

    public List<string> Teams = new List<string>();

    public string CurrentRelease { get; set; } = "";

    public string ItemURL {get; set;}


    public EstimationsModel(IConfiguration configuration)
    {
        
    }

    public void OnGet()
    {

        CurrentRelease = BusinessLogic.Releases.GetCurrentRelease();
        DateTime dt = BusinessLogic.Releases.GetDateOfCurrentRelease();
        Releases = BusinessLogic.Releases.GetReleases(dt.AddMonths(-1), dt.AddMonths(3));
        Teams = BusinessLogic.Teams.QueryTeams().ToList();
        ItemURL = Utils.Config.ItemURL;

     
    }

                       
}
