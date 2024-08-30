using System;
using System.Collections.Generic;

public static class DataStore
{
    public static Dictionary<string, List<double>> portValues = new Dictionary<string, List<double>>();
    public static Dictionary<string, List<DateTime>> portDates = new Dictionary<string, List<DateTime>>();
    public static Dictionary<string, List<string>> portStrings = new Dictionary<string, List<string>>();
    public static Dictionary<string, List<DateTime>> dates = new Dictionary<string, List<DateTime>>();
    public static Dictionary<string, List<string>> strings = new Dictionary<string, List<string>>();
    public static Dictionary<string, List<double>> values = new Dictionary<string, List<double>>();

    public static void Initialize()
    {
        portValues.Clear();
        portDates.Clear();
        portStrings.Clear();
        dates.Clear();
        strings.Clear();
        values.Clear();
    }
} 