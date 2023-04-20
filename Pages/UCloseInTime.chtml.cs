using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Enteties;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using TFSViewer.BusinessLogic;
using TFSViewer.Enteties;

namespace TFSViewer.Pages;

public class UCloseInTimeModule : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string date { get; set; }

    [BindProperty(SupportsGet = true)]
    public string area { get; set; }

    public List<Release> Releases;

    public List<string> Teams = new List<string>();

    public string CurrentRelease { get; set; } = "";



    public void OnGet()
    {

        CurrentRelease = BusinessLogic.Releases.GetCurrentRelease();
        DateTime dt = BusinessLogic.Releases.GetDateOfCurrentRelease();

        Teams = BusinessLogic.Teams.QueryTeams().ToList();
        Releases = BusinessLogic.Releases.GetReleases(4, 0);
    }

}
