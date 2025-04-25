using CpuTick.Models;
using CpuTick.Tools;
using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("This is a simple console application that benchmarks CPU performance by simulating task execution.");

        var cpuTools = new CpuTools();
        var cpuInfo = cpuTools.GetCpuInfo();

        bool keepRunning = true;

        while (keepRunning)
        {
            int tasksCount = PromptForTaskCount(cpuInfo);

            RunBenchmark(tasksCount);

            Console.WriteLine("\nDo you want to run another test? (Y/N):");
            var input = Console.ReadLine()?.Trim().ToLower();

            if (input != "y" && input != "yes")
                keepRunning = false;
        }

        Console.WriteLine("Goodbye Friend");
        Console.ReadKey();
    }

    static int PromptForTaskCount(CpuModel cpuInfo)
    {
        Console.WriteLine("\nPlease enter the number of tasks:");

        if (!int.TryParse(Console.ReadLine(), out int tasksCount) || tasksCount <= 0)
        {
            Console.WriteLine("Invalid input. Calculating a recommended task count based on your CPU...");
            int cyclesPerTask = 1;
            tasksCount = cpuInfo.Cores * cpuInfo.MaxClockSpeed * 60 / cyclesPerTask;
            Console.WriteLine($"Recommended Task Count: {tasksCount}");
        }

        return tasksCount;
    }

    static void RunBenchmark(int tasksCount)
    {
        var stopwatch = new Stopwatch();
        var heavyTask = new HeavyTask();

        stopwatch.Start();
        heavyTask.Run(tasksCount);
        stopwatch.Stop();

        Console.WriteLine($"\nTasks Completed: {tasksCount} | Elapsed Time: {stopwatch.Elapsed.TotalMilliseconds:N0} ms");
    }
}
