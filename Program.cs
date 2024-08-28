using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        //HARBOUR DUES

        List<DateTime> undocked = new List<DateTime>();
        undocked.Add(new DateTime(2024, 1, 22, 13, 30, 0));
        //undocked.Add(new DateTime(2024, 1, 24, 17, 50, 0));
        
        List<DateTime> docked = new List<DateTime>();
        docked.Add(new DateTime(2024, 1, 10, 9, 37, 0));
        //docked.Add(new DateTime(2024, 1, 12, 10, 20, 0));

        double amt = 0.0;
        double qty = 0.0;
        double cost1 = 0.079; //25,000 - 45,000 per 12h
        double cost2 = 0.763; //45,001 - 90,000 per 12h
        double cost3 = 73.450; //above 90,000 per 12h
        double GT = 300;
        double time = 0.0;

        for(int i= 0; i < undocked.Count; i++){
        time += Math.Ceiling(((undocked[i]-docked[i]).TotalHours)/12); 
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

        Console.WriteLine($"Return Qty: {qty}, Amt: {amt}");
    }
}