using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace XMLParsingExample
{
    class Program
    {
        static DateTime? ParseDateTime(string datetimeStr)
        {
            if (DateTime.TryParseExact(datetimeStr, "yyyy-MM-ddTHH:mm:ss.fff", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDateTime))
            {
                return parsedDateTime;
            }
            return null;
        }

        static void Main(string[] args)
        {
            // Initialize the DataStore
            DataStore.Initialize();

            // Path to the XML file
            string xmlFilePath = "xml_short.xml";

            XNamespace ns = "http://www.mscgva.ch/service/vci/2010/11/16";

            // Parse XML file
            XDocument doc = XDocument.Load(xmlFilePath);

            // Parse XML and populate DataStore
            foreach (XElement item in doc.Descendants(ns + "item"))
            {
                string? tag = item.Attribute("tag")?.Value;
                string? value = item.Attribute("value")?.Value;
                string? type = item.Attribute("type")?.Value;
                int index = Convert.ToInt32(item.Attribute("index")?.Value ?? "0");

                if (tag == null || value == null || type == null) continue;

                switch (tag)
                {
                    case "Length Over All":
                        if (type == "N" && double.TryParse(value, out double loaValue))
                            DataStore.portValues[tag] = new List<double> { loaValue };
                        break;
                    case "Vessel undocked":
                        if (type == "D" && ParseDateTime(value) is DateTime udValue)
                        {
                            DataStore.portDates[tag] = new List<DateTime> { udValue };
                            DataStore.dates[tag] = new List<DateTime> { udValue };
                        }
                        break;
                    case "Docked [All Fast] at terminal":
                        if (type == "S")
                            DataStore.portStrings[tag] = new List<string> { value };
                        else if (type == "D" && ParseDateTime(value) is DateTime dsValue)
                            DataStore.dates[tag] = new List<DateTime> { dsValue };
                        break;
                    case "Husbandry Item":
                        if (!DataStore.strings.ContainsKey(tag))
                            DataStore.strings[tag] = new List<string>();
                        DataStore.strings[tag].Add(value);
                        break;
                    case "Husbandry Value":
                        if (type == "N" && double.TryParse(value, out double hvValue))
                        {
                            if (!DataStore.values.ContainsKey(tag))
                                DataStore.values[tag] = new List<double>();
                            DataStore.values[tag].Add(hvValue);
                        }
                        break;
                }
            }

            // Calculate harbor dues
            var result = CalculateHarborDues();

            // Output only the required results
            Console.WriteLine($"Harbor Dues Quantity: {result.Quantity}");
            Console.WriteLine($"Harbor Dues Amount: {result.Amount:F2} {result.Currency}");
        }

        static (double Quantity, double Amount, string Currency) CalculateHarborDues()
        {
            double amt = 0.0;
            double qty = 0.0;
            double cost1 = 0.079; //25,000 - 45,000 per 12h
            double cost2 = 0.763; //45,001 - 90,000 per 12h
            double cost3 = 73.450; //above 90,000 per 12h
            
            // Check if "Length Over All" exists in portValues
            if (!DataStore.portValues.TryGetValue("Length Over All", out var gtList) || gtList.Count == 0)
            {
                Console.WriteLine("Error: 'Length Over All' data is missing.");
                return (0, 0, "USD");
            }
            double GT = gtList[0];

            double time = 0.0;

            // Check if both required date fields exist
            if (DataStore.dates.TryGetValue("Vessel undocked", out var undockedDates) &&
                DataStore.dates.TryGetValue("Docked [All Fast] at terminal", out var dockedDates))
            {
                for (int i = 0; i < Math.Min(undockedDates.Count, dockedDates.Count); i++)
                {
                    time += Math.Ceiling((undockedDates[i] - dockedDates[i]).TotalHours / 12);
                }
            }
            else
            {
                Console.WriteLine("Error: Required date information is missing.");
                return (0, 0, "USD");
            }

            if (GT > 250 && GT <= 45000)
            {
                amt = cost1 * time * GT;
            }
            else if (GT > 45000 && GT <= 90000)
            {
                amt = cost2 * time * GT;
            }
            else if (GT > 90000)
            {
                amt = cost3 * time * GT;
            }

            if (amt > 0)
            {
                qty = 1;
            }

            return (qty, amt, "USD");
        }
    }
}
