namespace TFSViewer.Enteties;

public class Release
{
    public string ReleaseName {get; set;}

    public DateTime StartDate {get; set;}

    public DateTime EndDate {get; set;}

    public List<String> Iterations {get; set;}

}