using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enteties;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using TFSViewer.BusinessLogic;

namespace TFSViewer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BugsController : ControllerBase
    {
        public List<WorkItem> GetBugsFromHighEnvironment(string environment, string areaPath = "", string date = "")
        {
            DateTime? currentDate = null;
            if (!string.IsNullOrWhiteSpace(date))
            {
                currentDate = DateTime.ParseExact(date, "yyyy-MM-dd", null);
            }

            IList<WorkItem> workItems;
            if (environment == "uat")
            {
                workItems = Bugs.GetUATBugs(areaPath, currentDate);
            }
            else
            {
                workItems = Bugs.GetProductionBugs(areaPath, currentDate);
            }

            return workItems.ToList();
        }
    }
}
