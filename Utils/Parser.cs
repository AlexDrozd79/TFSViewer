namespace TFSViewer.Utils;

public static class Parser
{
    public static string GetStringValue(IDictionary<string, object> fields, string fieldName)
    {
        string result = "";
        object val;
        if (fields.TryGetValue(fieldName, out val))
        {
            result = val?.ToString();
        }
        return (result ?? "").ToLower().Trim();
    }

    public static float GetFloatValue(IDictionary<string, object> fields, string fieldName)
    {
        float result = 0;
        object val;
        if (fields.TryGetValue(fieldName, out val))
        {
            float.TryParse(val?.ToString(), out result);
        }
        return result;
    }
}