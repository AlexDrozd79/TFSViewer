namespace TFSViewer.BusinessLogic;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using TFSViewer.Utils;

public class Teams
{
    private static string[] teams { get; set; }

    private static object locker = new Object();

    static Teams()
    {
        if (teams == null)
        {
            string path = AppContext.BaseDirectory;
            teams = File.ReadAllLines(path + "data\\teams.txt");
        }

    }


    public static IList<string> QueryTeams()
    {
        return teams;
    }

}