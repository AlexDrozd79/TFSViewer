namespace Enteties;

using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

public class FeaturesInfo
{
    private List<WorkItem> Features = new List<WorkItem>();

    public FeaturesInfo(List<WorkItem> features)
    {
        Features = features;
    }

    public List<WorkItem> All
    {
        get
        {
            return Features.ToList();
        }
    }

    public List<WorkItem> WaitingForEvaluation
    {
        get
        {
            // return Features.Where(f => GetStringValue(f.Fields, "System.State") == "new")
            // .Where(f => GetStringValue(f.Fields, "System.Tags").Contains("feature evaluation") && GetStringValue(f.Fields, "NG.QAEstimation") == ""
            //     && GetStringValue(f.Fields, "NG.NetEstimation") == "" && GetStringValue(f.Fields, "NG.DBAEstimation") == ""
            //     && GetStringValue(f.Fields, "NG.FrontendEstimation") == "" && GetStringValue(f.Fields, "NG.QAAutomationEstimation") == "").ToList();

                 return Features.Where(f => GetStringValue(f.Fields, "System.State") == "new")
            .Where(f => GetStringValue(f.Fields, "NG.QAEstimation") == ""
                && GetStringValue(f.Fields, "NG.NetEstimation") == "" && GetStringValue(f.Fields, "NG.DBAEstimation") == ""
                && GetStringValue(f.Fields, "NG.FrontendEstimation") == "" && GetStringValue(f.Fields, "NG.QAAutomationEstimation") == "").ToList();
        }
    }

    public List<WorkItem> WaitingForGrooming
    {
        get
        {
            // return   Features.Where(f => GetStringValue(f.Fields, "System.State" ) == "new")
            // .Where(f => GetStringValue(f.Fields, "System.Tags").Contains("feature evaluation") && (GetStringValue(f.Fields, "NG.QAEstimation") != "" 
            //     || GetStringValue(f.Fields, "NG.NetEstimation") != "" || GetStringValue(f.Fields, "NG.DBAEstimation") != "" 
            //     || GetStringValue(f.Fields, "NG.FrontendEstimation")!="" || GetStringValue(f.Fields, "NG.QAAutomationEstimation")!="")).ToList();

            return Features.Where(f => GetStringValue(f.Fields, "System.State") == "new")
            .Where(f =>  GetStringValue(f.Fields, "NG.QAEstimation") != ""
                || GetStringValue(f.Fields, "NG.NetEstimation") != "" || GetStringValue(f.Fields, "NG.DBAEstimation") != ""
                || GetStringValue(f.Fields, "NG.FrontendEstimation") != "" || GetStringValue(f.Fields, "NG.QAAutomationEstimation") != "")
                .Where(f => !GetStringValue(f.Fields, "System.Tags").Contains("groomed"))
                .ToList();
        }
    }

    public List<WorkItem> Ready
    {
        get
        {
            return Features.Where(f => GetStringValue(f.Fields, "System.State") == "ready"
                || (GetStringValue(f.Fields, "System.State") == "new" && GetStringValue(f.Fields, "System.Tags").Contains("groomed"))).ToList();
        }
    }


    public List<WorkItem> WithChildrens
    {
        get
        {
            return Features.Where(f => GetStringValue(f.Fields, "System.State") == "ready"
                && f.Relations != null).ToList();
        }
    }


    private string GetStringValue(IDictionary<string, object> fields, string fieldName)
    {
        string? result = "";
        object? val;
        if (fields.TryGetValue(fieldName, out val))
        {
            result = val?.ToString();
        }
        return (result ?? "").ToLower().Trim();
    }
}