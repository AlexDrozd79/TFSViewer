using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Enteties;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace TFSViewer.Pages;

public class IndexModel : PageModel
{
    private readonly IConfiguration Configuration;
    public FeaturesInfo info;

    public IndexModel(IConfiguration configuration)
    {
        Configuration = configuration;
        info = new FeaturesInfo(new List<WorkItem>());
    }

    public void OnGet()
    {
        BusinessLogic.Features features = new BusinessLogic.Features(Configuration);
        var workItems = features.QueryFeatures("NeoAppAgile");
        info = new FeaturesInfo( workItems.ToList());
    }
}
