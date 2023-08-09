using System;

[Serializable]
public struct Ship
{
    public int size;
    public int amount;

    public Ship(int size, int amount)
    {
        this.size = size;
        this.amount = amount;
    }

    public void Deconstruct(out int outSize, out int outAmount)
    {
        outSize = size;
        outAmount = amount;
    }

    public static string FormatInfo(Ship ship)
    {
        return $"{ship.amount}x{ship.size}";
    }
}