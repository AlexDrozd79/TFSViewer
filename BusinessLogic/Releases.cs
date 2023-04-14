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
using TFSViewer.Enteties;
using Microsoft.Data.Sqlite;

public class Releases
{
    private static string CurrentRelease = "";

    private static List<Release> _releases = new List<Release>();

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

    public static List<Release> GetReleases(short beforeCurrent, short afterCurrent)
    {
        InitReleasesIfNeeded();
        string current = Releases.GetCurrentRelease();
        int index = _releases.FindIndex(0, _releases.Count, a => a.ReleaseName.ToLower() == current.ToLower());
        return _releases.Take(new Range(index - beforeCurrent, index + afterCurrent + 1)).ToList();

    }

    private static void InitReleasesIfNeeded()
    {
        if (_releases.Count == 0)
        {
            lock (locker)
            {
                if (_releases.Count == 0)
                {
                    using (var connection = new SqliteConnection("Data Source=" + AppContext.BaseDirectory + "data\\tfs.db"))
                    {
                        connection.Open();

                        var command = connection.CreateCommand();
                        command.CommandText =
                        @"
                            SELECT Release, StartDate, EndDate
                            FROM Releases
                        ";

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                _releases.Add(new Release()
                                {
                                    ReleaseName = reader.GetString(0),
                                    StartDate = reader.GetDateTime(1),
                                    EndDate = reader.GetDateTime(2)
                                });
                            }
                        }
                    }
                }
            }
        }
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