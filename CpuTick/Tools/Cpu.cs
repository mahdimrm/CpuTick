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
        if (!IsWindows())
            Console.WriteLine("The Operating System is not Windows.");

        var cpuInfo = new CpuModel
        {
            Architecture = GetCpuArchitecture(),
            Cores = GetSystemCpuCoreCount(),
            MaxClockSpeed = GetCpuMaxClockSpeed(),
            CacheSize = GetCpuCacheSize(),
            Load = GetCpuLoad()
        };

        PrintCpuInfo(cpuInfo);

        return cpuInfo;
    }

    private int GetSystemCpuCoreCount()
    {
        Console.WriteLine("Getting system CPU core count...");
        int coreCount = 0;

        try
        {
            foreach (var obj in new ManagementObjectSearcher(Win32_Processor).Get())
            {
                if (int.TryParse(obj["NumberOfCores"]?.ToString(), out int cores))
                    coreCount += cores;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving core count: {ex.Message}");
        }

        return coreCount;
    }

    private float GetCpuLoad()
    {
        try
        {
            var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            _ = cpuCounter.NextValue();
            Thread.Sleep(1000);
            return cpuCounter.NextValue();
        }
        catch
        {
            Console.WriteLine("Unable to read CPU load.");
            return -1f;
        }
    }

    private int GetCpuMaxClockSpeed()
    {
        Console.WriteLine("Getting CPU max clock speed...");
        int maxClockSpeed = 0;

        try
        {
            foreach (var obj in new ManagementObjectSearcher(MaxClockSpeed).Get())
            {
                if (int.TryParse(obj["MaxClockSpeed"]?.ToString(), out int speed))
                    maxClockSpeed = speed;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving clock speed: {ex.Message}");
        }

        return maxClockSpeed;
    }

    private CacheSize GetCpuCacheSize()
    {
        var cache = new CacheSize();
        try
        {
            var searcher = new ManagementObjectSearcher(VmiNameSpaces.CacheSize, Vmi.CacheSize);
            foreach (ManagementObject obj in searcher.Get())
            {
                cache.L2CacheSize = int.TryParse(obj["L2CacheSize"]?.ToString(), out int l2) ? l2 : 0;
                cache.L3CacheSize = int.TryParse(obj["L3CacheSize"]?.ToString(), out int l3) ? l3 : 0;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving cache size: {ex.Message}");
        }

        return cache;
    }

    private string GetCpuArchitecture()
        => RuntimeInformation.ProcessArchitecture.ToString();

    private bool IsWindows()
        => OperatingSystem.IsWindows();

    private void PrintCpuInfo(CpuModel cpu)
    {
        Console.WriteLine("=======================================");
        Console.WriteLine("               CPU INFO               ");
        Console.WriteLine("=======================================");
        Console.WriteLine($"Architecture      : {cpu.Architecture}");
        Console.WriteLine($"Cores             : {cpu.Cores}");
        Console.WriteLine($"Max Clock Speed   : {cpu.MaxClockSpeed} MHz");
        Console.WriteLine("---------------------------------------");
        Console.WriteLine("Cache Sizes:");
        Console.WriteLine($"  L2 Cache        : {cpu.CacheSize.L2CacheSize} KB");
        Console.WriteLine($"  L3 Cache        : {cpu.CacheSize.L3CacheSize} KB");
        Console.WriteLine("---------------------------------------");
        Console.WriteLine($"Current Load      : {cpu.Load}%");
        Console.WriteLine("=======================================");
    }
}
