using CpuTick.Constants;
using CpuTick.Models;
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;

namespace CpuTick.Tools;

public class CpuTools
{
    public CpuModel GetCpuInfo()
    {
        bool isWindows = IsWinows();
        if (!isWindows)
            Console.WriteLine("The Operating System Is Not Windows");

        var cpuInfo = new CpuModel
        {
            Architecture = GetCpuArchitecture(),
            Cores = GetSystemCpuCoreCount(),
            MaxClockSpeed = GetCpuMaxClockSpeed(),
            CacheSize = GetCPUCacheSize(),
            Load = GetCpuLoad()
        };

        Console.WriteLine("=======================================");
        Console.WriteLine("               CPU INFO               ");
        Console.WriteLine("=======================================");
        Console.WriteLine($"Architecture      : {cpuInfo.Architecture}");
        Console.WriteLine($"Cores             : {cpuInfo.Cores}");
        Console.WriteLine($"Max Clock Speed   : {cpuInfo.MaxClockSpeed} MHz");
        Console.WriteLine($"---------------------------------------");
        Console.WriteLine($"Cache Sizes");
        Console.WriteLine($"  L2 Cache        : {cpuInfo.CacheSize.L2CacheSize} KB");
        Console.WriteLine($"  L3 Cache        : {cpuInfo.CacheSize.L3CacheSize} KB");
        Console.WriteLine($"---------------------------------------");
        Console.WriteLine($"Current Load      : {cpuInfo.Load}%");
        Console.WriteLine("=======================================");

        return cpuInfo;

    }
    private int GetSystemCpuCoreCount()
    {
        Console.WriteLine("Geting System Cpu Cores Count");
        var coreCount = 0;
        var searcher = new ManagementObjectSearcher(Win32_Processor).Get();
        foreach (var obj in searcher)
        {
            string? numberOfCores = obj["NumberOfCores"].ToString();
            coreCount += int.Parse(numberOfCores);
        }
        return coreCount;
    }

    private float GetCpuLoad()
    {
        PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        float cpuUsage = cpuCounter.NextValue();
        Thread.Sleep(1000);
        return cpuCounter.NextValue();
    }

    private int GetCpuMaxClockSpeed()
    {
        Console.WriteLine("Getting Cpu MaxClock Speed");
        int maxClockSpeed = 0;

        ManagementObjectCollection searcher = new ManagementObjectSearcher(MaxClockSpeed).Get();

        foreach (var obj in searcher)
            maxClockSpeed = int.Parse(obj["MaxClockSpeed"].ToString());

        return maxClockSpeed;
    }
    private CacheSize GetCPUCacheSize()
    {
        var searcher = new ManagementObjectSearcher(VmiNameSpaces.CacheSize, VmiQueries.CacheSize);
        var cache = new CacheSize()
        {
            L2CacheSize = 0,
            L3CacheSize = 0
        };
        foreach (ManagementObject queryObj in searcher.Get())
        {
            cache.L2CacheSize = queryObj["L2CacheSize"] != null ? int.Parse(queryObj["L2CacheSize"].ToString()) : 0;
            cache.L3CacheSize = queryObj["L3CacheSize"] != null ? int.Parse(queryObj["L3CacheSize"].ToString()) : 0;
        }
        return cache;
    }

    private string GetCpuArchitecture()
        => RuntimeInformation.ProcessArchitecture.ToString();

    private bool IsWinows()
        => OperatingSystem.IsWindows();
}
