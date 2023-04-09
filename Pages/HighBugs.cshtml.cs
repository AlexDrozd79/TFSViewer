using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Enteties;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using TFSViewer.BusinessLogic;

namespace TFSViewer.Pages;

public class HighBugs : PageModel
{

    public List<string> Teams = new List<string>();

    public void OnGet()
    {
        Teams = BusinessLogic.Teams.QueryTeams().ToList();
    }


}
