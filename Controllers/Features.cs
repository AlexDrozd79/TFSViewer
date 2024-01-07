using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enteties;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using TFSViewer.BusinessLogic;
using TFSViewer.Utils;

namespace TFSViewer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeaturesController : ControllerBase
    {
       
        public FeaturesInfo GetFeatures(string release = "", string date = "", string areapath = "")
        {
            string currentRelease = release;
            if (string.IsNullOrWhiteSpace(currentRelease))
            {
                currentRelease = Releases.GetCurrentRelease();
            }

            DateTime currentDate = DateTime.Today;
            if (!string.IsNullOrWhiteSpace(date))
            {
                currentDate = DateTime.ParseExact(date, "yyyy-MM-dd", null);
            }

            var workItems = Features.QueryFeatures(currentRelease, currentDate, areapath);
            return new FeaturesInfo(workItems.ToList());

        }

        [HttpGet("GetFeaturesRecursive")]
        public List<Enteties.FeatureEx> GetFeaturesRecursive(string release = "", string date = "", string areapath = "")
        {
            string currentRelease = release;
            if (string.IsNullOrWhiteSpace(currentRelease))
            {
                currentRelease = Releases.GetCurrentRelease();
            }

            DateTime currentDate = DateTime.Today;
            if (!string.IsNullOrWhiteSpace(date))
            {
                currentDate = DateTime.ParseExact(date, "yyyy-MM-dd", null);
            }

            List<WorkItem> workItems = Features.QueryFeaturesRecursive(currentRelease, currentDate, areapath).ToList();

            List<Enteties.FeatureEx> result = workItems.Where(f => Parser.GetStringValue(f.Fields, "System.WorkItemType") == "feature").Select(f => new Enteties.FeatureEx(f.Id.Value, workItems)).ToList();

            return result;

        }
    }
}
