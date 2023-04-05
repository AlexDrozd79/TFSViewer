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

public class Releases
{
    private static string CurrentRelease = "";

    private static object locker = new Object();
    public static List<string> GetReleases(DateTime? startDate = null, DateTime? endDate = null)
    {
        List<string> releases = new List<string>();
        DateTime fromDate = GetDateOfCurrentRelease();
        DateTime toDate = fromDate.AddMonths(3);

        if (startDate.HasValue)
        {
            fromDate = startDate.Value;
        }

        if (endDate.HasValue)
        {
            toDate = endDate.Value;
        }

        for (DateTime dt = fromDate; dt <= toDate; dt = dt.AddMonths(1))
        {
            releases.Add(dt.Year + "-" + (dt.Month.ToString().Length == 1 ? "0" + dt.Month : dt.Month));
        }

        return releases;
    }

    public static string GetCurrentRelease()
    {
        if (string.IsNullOrEmpty(CurrentRelease))
        {
            lock (locker)
            {
                if (string.IsNullOrEmpty(CurrentRelease))
                {
                    CurrentRelease = File.ReadAllText(AppContext.BaseDirectory + "data\\CurrentRelease.txt");
                }
            }
        }

        return CurrentRelease;
    }

    public static void SetCurrentRelease(string release)
    {
        lock (locker)
        {
            File.WriteAllText(AppContext.BaseDirectory + "data\\CurrentRelease.txt", release);
            CurrentRelease = release;
        }
    }

    public static DateTime GetDateOfCurrentRelease()
    {
        string currentRelease = GetCurrentRelease();
        string[] parts = currentRelease.Split("-");
        return new DateTime(int.Parse(parts[0]), int.Parse(parts[1]), 1);
    }
}