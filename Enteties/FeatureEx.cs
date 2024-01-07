using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using TFSViewer.Utils;

namespace TFSViewer.Enteties;

public class FeatureEx
{
    public int ID { get; set; }

    public string Title { get; set; }

    public float NETF { get; set; }

    public float NETUS { get; set; }

    public float NETTask { get; set; }

    public float DBAF { get; set; }

    public float DBAUS { get; set; }

    public float DBATask { get; set; }

    public float FEF { get; set; }

    public float FEUS { get; set; }

    public float FETask { get; set; }

    public float QAF { get; set; }

    public float QAUS { get; set; }

    public float QATask { get; set; }

    public float QAAF { get; set; }

    public float QAAUS { get; set; }

    public float QAATask { get; set; }

    public FeatureEx(int FeatureID, IList<WorkItem> items)
    {
        WorkItem feature = items.Where(i => i.Id == FeatureID).First();
        List<WorkItem> childUS = items.Where(u => Parser.GetStringValue(u.Fields, "System.WorkItemType") == "user story").Where(i => Parser.GetStringValue(i.Fields, "System.Parent") == FeatureID.ToString()).ToList();
        List<WorkItem> childTasks = items.Where(t => Parser.GetStringValue(t.Fields, "System.WorkItemType") == "task").Where(i => childUS.Exists(c => Parser.GetStringValue(c.Fields, "System.Id") == Parser.GetStringValue(i.Fields, "System.Parent"))).ToList();

        ID = FeatureID;
        Title = Parser.GetStringValue(feature.Fields, "System.Title");

        NETF = Parser.GetFloatValue(feature.Fields, "NG.NetEstimation");
        FEF = Parser.GetFloatValue(feature.Fields, "NG.FrontendEstimation");
        DBAF = Parser.GetFloatValue(feature.Fields, "NG.DBAEstimation");
        QAF = Parser.GetFloatValue(feature.Fields, "NG.QAEstimation");
        QAAF = Parser.GetFloatValue(feature.Fields, "NG.QAAutomationEstimation");

        if (childUS.Count > 0)
        {
            NETUS = childUS.Sum(us => Parser.GetFloatValue(us.Fields, "NG.NetEstimation"));
            FEUS = childUS.Sum(us => Parser.GetFloatValue(us.Fields, "NG.FrontendEstimation"));
            DBAUS = childUS.Sum(us => Parser.GetFloatValue(us.Fields, "NG.DBAEstimation"));
            QAUS = childUS.Sum(us => Parser.GetFloatValue(us.Fields, "NG.QAEstimation"));
            QAAUS = childUS.Sum(us => Parser.GetFloatValue(us.Fields, "NG.QAAutomationEstimation"));

            if (childTasks.Count > 0)
            {
                NETTask = childTasks.Where(task => Parser.GetStringValue(task.Fields, "System.Title").Trim().ToUpper().StartsWith(".NET")).Sum(task => Parser.GetFloatValue(task.Fields, "Microsoft.VSTS.Scheduling.OriginalEstimate"));
                FETask = childTasks.Where(task => Parser.GetStringValue(task.Fields, "System.Title").Trim().ToUpper().StartsWith("FE")).Sum(task => Parser.GetFloatValue(task.Fields, "Microsoft.VSTS.Scheduling.OriginalEstimate"));
                DBATask = childTasks.Where(task => Parser.GetStringValue(task.Fields, "System.Title").Trim().ToUpper().StartsWith("DB")).Sum(task => Parser.GetFloatValue(task.Fields, "Microsoft.VSTS.Scheduling.OriginalEstimate"));
                QATask = childTasks.Where(task => Parser.GetStringValue(task.Fields, "System.Title").Trim().ToUpper().StartsWith("QA ")).Sum(task => Parser.GetFloatValue(task.Fields, "Microsoft.VSTS.Scheduling.OriginalEstimate"));
                QAATask = childTasks.Where(task => Parser.GetStringValue(task.Fields, "System.Title").Trim().ToUpper().StartsWith("QAA")).Sum(task => Parser.GetFloatValue(task.Fields, "Microsoft.VSTS.Scheduling.OriginalEstimate"));

                NETTask = NETTask/7f;
                FETask = FETask/7f;
                DBATask = DBATask/7f;
                QATask = QATask/7f;
                QAATask = QAATask/7f;


            }
        }

    }


}