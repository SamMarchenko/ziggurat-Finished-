using System;

namespace Ziggurat
{
    public enum EUnitType
    {
        Blue,
        Red,
        Green,
    }

    [Flags]
    public enum EIgnoreAxisType
    {
        None = 0,
        X = 1,
        Y = 2,
        Z = 4
    }
}