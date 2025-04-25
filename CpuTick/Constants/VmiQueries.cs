namespace CpuTick.Constants;

public static class VmiQueries
{
    public static string Win32_Processor { get; } = "Select * from Win32_Processor";
    public static string MaxClockSpeed = "select MaxClockSpeed from Win32_Processor";
    public static string CacheSize = "SELECT * FROM Win32_Processor";
}

public static class VmiNameSpaces
{
    public static string CacheSize { get; set; } = "root\\CIMV2";
}