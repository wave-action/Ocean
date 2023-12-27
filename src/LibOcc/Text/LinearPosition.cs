namespace LibOcc.Text;

public readonly struct LinearPosition(uint start, uint end)
{
    public uint Start { get; init; } = start;
    public uint End { get; init; } = end;
}