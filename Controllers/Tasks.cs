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
    public class TasksController : ControllerBase
    {
        public List<WorkItem> GetTasks(string release = "", string date = "", string areapath = "")
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

            string currentAreaPath = "NeoAppAgile\\Lotteries";
            if (!string.IsNullOrWhiteSpace(areapath))
            {
                currentAreaPath = areapath;
            }

            var workItems = Tasks.QueryTasks(currentRelease, currentDate, currentAreaPath);
            return workItems.ToList();

        }
    }
}
