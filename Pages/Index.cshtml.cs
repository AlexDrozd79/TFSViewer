using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Enteties;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using TFSViewer.BusinessLogic;

namespace TFSViewer.Pages;

public class IndexModel : PageModel
{
    private readonly IConfiguration Configuration;

    public List<string> Releases = new List<string>();

    public string CurrentRelease {get; set;} = "";

    public IndexModel(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void OnGet()
    {

        CurrentRelease = BusinessLogic.Releases.GetCurrentRelease();
        Releases = BusinessLogic.Releases.GetReleases();

        
    }

}
