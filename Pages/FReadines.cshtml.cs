using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Enteties;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using TFSViewer.BusinessLogic;
using TFSViewer.Enteties;

namespace TFSViewer.Pages;

public class FReadinessModel : PageModel
{
    public List<Release> Releases = new List<Release>();
    public List<string> Teams = new List<string>();
    public string CurrentRelease {get; set;} = "";


    public void OnGet()
    {
        CurrentRelease = BusinessLogic.Releases.GetCurrentRelease();
        Releases = BusinessLogic.Releases.GetReleases(3, 1);
        Teams = BusinessLogic.Teams.QueryTeams().ToList(); 
    }

}
