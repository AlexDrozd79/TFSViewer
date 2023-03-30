using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Enteties;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

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

        BusinessLogic.Releases releases = new BusinessLogic.Releases(Configuration);
        CurrentRelease = releases.GetCurrentRelease("NeoAppAgile");
        Releases = releases.GetReleases("NeoAppAgile");

        
    }

}
