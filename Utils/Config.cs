namespace TFSViewer.Utils;

using System;

public static class Config
{
    public static void Init(IConfiguration config)
    {
        User = config.GetValue<string>("user") ?? "";
        Project = config.GetValue<string>("project") ?? "";
        Password = config.GetValue<string>("password") ?? "";
        URI = config.GetValue<string>("uri") ?? "";
        PersonalAccessToken = config.GetValue<string>("personalAccessToken") ?? "";
    }
    public static string Project { get; set; } = "";

    public static string User { get; set; } = "";

    public static string Password { get; set; } = "";

    public static string URI { get; set; } = "";

    public static string PersonalAccessToken { get; set; } = "";



}