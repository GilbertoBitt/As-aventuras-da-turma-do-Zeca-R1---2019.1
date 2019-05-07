using System;
using System.Collections;
using System.Collections.Generic;

public static class EnumExtensions
{
    public static T RandomEnumValue<T> ()
    {
        var v = Enum.GetValues (typeof (T));
        return (T) v.GetValue (new Random ().Next(v.Length));
    }
}
