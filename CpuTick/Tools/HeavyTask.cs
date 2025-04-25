namespace CpuTick.Tools
{
    public class HeavyTask
    {
        public void Run(int tasksCount)
        {
            long count = 0;
            Parallel.For(2, tasksCount, (number) =>
            {
                if (IsPrime(number))
                    Interlocked.Increment(ref count);
            });
        }

        public bool IsPrime(long number)
        {
            if (number < 2) return false;
            for (long i = 2; i * i <= number; i++)
            {
                if (number % i == 0) return false;
            }
            return true;
        }
    }
}
