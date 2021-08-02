using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTime
{
    public static double GetTimestamp(DateTime value)
    {
        return Double.Parse(value.ToString("yyyyMMddHHmmss"));
    }

    public static DateTime getDateTimeFromString(string timeString)
    {
        DateTime dateFromString = DateTime.Parse(timeString, System.Globalization.CultureInfo.InvariantCulture);
        return dateFromString;
    }
}
