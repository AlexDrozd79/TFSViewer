using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enteties;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TFSViewer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeaturesController : ControllerBase
    {

        private readonly IConfiguration Configuration;

        public FeaturesController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // GET: api/TodoItems
        [HttpGet("{release?}")]
        public FeaturesInfo GetFeatures(string release = "")
        {

            BusinessLogic.Releases releases = new BusinessLogic.Releases(Configuration);
            string currentRelease = release;
            if (string.IsNullOrWhiteSpace(currentRelease))
            {
                currentRelease = releases.GetCurrentRelease("NeoAppAgile");
            }

            BusinessLogic.Features features = new BusinessLogic.Features(Configuration);
            var workItems = features.QueryFeatures("NeoAppAgile", currentRelease);
            return new FeaturesInfo(workItems.ToList());
        }



    }
}
