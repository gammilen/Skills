using System;

namespace Skills.Data
{
    public interface IPointsInfo
    {
        event Action AmountChanged;
        int Amount { get; }
    }
}