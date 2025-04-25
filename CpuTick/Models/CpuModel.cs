namespace CpuTick.Models;
public record CpuModel
{
    public string Architecture { get; set; }
    public int Cores { get; internal set; }
    public int MaxClockSpeed { get; internal set; }
    public CacheSize CacheSize { get; internal set; }
    public float Load { get; set; }
}
