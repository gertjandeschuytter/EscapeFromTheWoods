using DataLaag.ADO;
using DomeinLaag.Beheerders;
using DomeinLaag.Interfaces;
using DomeinLaag.Klassen;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class Program
    {
        #region Properties
        #endregion

        static void Main(string[] args)
        {
            Console.WriteLine("Start applicatie");
            Stopwatch stopwatch = new();
            stopwatch.Start();
            Bos bos1 = BosBeheerder.GenereerBos(1, 1000, 1000, new List<string> { "Filip", "Janne", "Loewis", "Jan" });
            Bos bos2 = BosBeheerder.GenereerBos(2, 500, 500, new List<string> { "Piet", "Jean-Pierre", "Patrick", "Tom" });
            Bos bos3 = BosBeheerder.GenereerBos(3, 750, 500, new List<string> { "Pol", "Johannes", "Flosj", "Morane" });
            Bos bos4 = BosBeheerder.GenereerBos(4, 800, 600, new List<string> { "Heidi", "Thomas", "Simon", "Suzanne" });

            Console.WriteLine("Start - Genereer mappensctructuur");
            ApplicatieBeheerder.GenereerDirectories();
            Console.WriteLine("Einde - Genereer mappensctructuur");

            List<Task> tasks1 = new();
            tasks1.Add(Task.Run(() => ApplicatieBeheerder.Process(bos1)));
            tasks1.Add(Task.Run(() => ApplicatieBeheerder.Process(bos2)));
            tasks1.Add(Task.Run(() => ApplicatieBeheerder.Process(bos3)));
            tasks1.Add(Task.Run(() => ApplicatieBeheerder.Process(bos4)));
            Task.WaitAll(tasks1.ToArray());
            stopwatch.Stop();
            Console.WriteLine($"Einde applicatie - {stopwatch.Elapsed}");
        }
    }
}
