using CpuTick.Models;
using CpuTick.Tools;
using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("This is a simple console application that allows you to know how long it takes a CPU to perform tasks based on its capabilities.");

        bool isRunning = true;
        int currentStep = 1;

        CpuTools cpuTools = new CpuTools();

        CpuModel cpuInfo = cpuTools.GetCpuInfo();

        int tasksCount = 0;

        while (isRunning)
        {
            switch (currentStep)
            {
                case 1:
                    Console.WriteLine("Please Enter Tasks Count");

                    tasksCount = int.Parse(Console.ReadLine());
                    if (tasksCount <= 0)
                    {
                        Console.WriteLine("Invalid task count. Calculating a recommended value...");
                        int cyclesPerTask = 1;
                        tasksCount = cpuInfo.Cores * cpuInfo.MaxClockSpeed * 60 / cyclesPerTask;
                        Console.WriteLine($"Estimated Task Count Based on CPU: {tasksCount}");
                    }

                    currentStep++;
                    break;

                case 2:
                    var stopWatch = new Stopwatch();
                    HeavyTask heavyTask = new HeavyTask();

                    stopWatch.Start();
                    heavyTask.Run(tasksCount);
                    stopWatch.Stop();

                    isRunning = false;
                    Console.WriteLine($"Tasks Completed: {tasksCount} | Elapsed Time: {stopWatch.Elapsed.TotalMilliseconds:N0} ms");
                    break;

                default:
                    Console.WriteLine("Unknow Step");
                    isRunning = false;
                    break;
            }
        }

        Console.ReadKey();
    }
}