using System;
using System.IO;

using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

// CosmosDB cost calculator console program.
//
// Usage:
//   dotnet run <your-input-file>
//   dotnet run specification1.txt
//   dotnet run specification2.txt
//
// Chris Joakim, Microsoft, 2020/10/26

namespace CJoakim.CosmosCalc
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                string infile = args[0];
                Console.WriteLine("Using input file: " + infile);

                Container currentContainer = null; 

                using (var sr = new StreamReader(infile))
                { 
                    while (sr.Peek() >= 0)
                    {
                        string[] tokens = sr.ReadLine().ToLower().Split(':');
                        if (tokens.Length == 2)
                        {
                            string key = tokens[0].Trim();
                            string value = tokens[1].Trim();

                            switch (key) {             
                                case "container": 
                                    currentContainer = new Container();
                                    currentContainer.name = value;
                                    break; 
                                case "ru_per_second":
                                    currentContainer.ruPerSecond = Int32.Parse(value);
                                    break;
                                case "region_count":
                                    currentContainer.regionCount = Int32.Parse(value);
                                    break;
                                case "availability_zone":
                                    if (value == "true") {
                                        currentContainer.availabilityZone = true;
                                    }
                                    if (value == "false") {
                                        currentContainer.availabilityZone = false;
                                    }    
                                    break; 
                                case "size_in_bytes": 
                                    currentContainer.SetSizeInBytes(Int64.Parse(value));
                                    break; 
                                case "size_in_mb": 
                                    currentContainer.SetSizeInMB(Double.Parse(value));
                                    break; 
                                case "size_in_gb": 
                                    currentContainer.sizeInGB = Double.Parse(value);
                                    break; 
                                case "size_in_tb": 
                                    currentContainer.SetSizeInTB(Double.Parse(value));
                                    break; 
                                case "max_historical_manual_ru": 
                                    currentContainer.maxHistoricalManualRu = Int32.Parse(value);
                                    break; 
                                case "max_historical_auto_ru": 
                                    currentContainer.maxHistoricalAutoRu = Int32.Parse(value);
                                    break; 

                                // The above case statements 'set' the state of the container, while
                                // the following case statements are used to trigger calculations.

                                case "calculate_min_ru":
                                    if (value == "true")
                                    {
                                        int min = currentContainer.CalculateMinRU();
                                        Console.WriteLine("calculated min RU: " + min);
                                    }
                                    break; 

                                case "calculate_costs":
                                    if (value == "true")
                                    {
                                        double costs = currentContainer.CalculateCosts();
                                        var options = new JsonSerializerOptions
                                        {
                                            WriteIndented = true,
                                        };
                                        Console.WriteLine(JsonSerializer.Serialize(currentContainer, options));
                                    }
                                    break; 

                                default: 
                                    break; 
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("No input file on the command line.");
            }
        }
    }
}
