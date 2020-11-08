using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using Xunit;

using CJoakim.CosmosCalc;

// Xunit unit tests for a Specification Matrix Scenario.
// Chris Joakim, Microsoft, 2020-11-08

namespace cosmos_calc.tests
{
    public class SpecMatrix30Test
    {
        [Fact]
        public void TestSpecMatrix30Test()
        {
            Container c = new Container();
            c.name             = "";
            c.provisioningType = "autoscale";
            c.replicationType  = "multi-master";
            c.availabilityZone = false;
            c.regionCount      = 3;
            c.sizeInGB         = 30000;
            c.ruPerSecond      = 333;
            double tolerance   = 0.000001;

            double costs = c.CalculateCosts();
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            Console.WriteLine(JsonSerializer.Serialize(c, options));


            // Assert.True(c.calculatedStoragePerMonth + tolerance > expectedStorageCostsPerMonth);
            // Assert.True(c.calculatedStoragePerMonth - tolerance < expectedStorageCostsPerMonth);

            // Assert.True(c.calculatedTotalPerMonth + tolerance > expectedTotalPerMonth);
            // Assert.True(c.calculatedTotalPerMonth - tolerance < expectedTotalPerMonth);
        }
    }
}

/*
Azure CosmosDB Cost Calculator Specification File

container:               container30
provisioning_type:       autoscale
replication_type:        multi-master
region_count:            3
availability_zone:       false
size_in_gb:              30000
replicated_gb_per_month: 3000.0
synapse_link_enabled:    true
calculate_costs:         true

*/

/*
{
  "name": "container30",
  "sizeInGB": 30000,
  "provisioningType": "autoscale",
  "availabilityZone": false,
  "synapseLinkEnabled": true,
  "replicationType": "multi-master",
  "regionCount": 3,
  "ruPerSecond": 0,
  "maxHistoricalManualRu": 0,
  "maxHistoricalAutoRu": 0,
  "replicatedGBPerMonth": 3000,
  "calculatedMinRU": 300000,
  "calculatedRatePer100RU": 0.016,
  "calculatedRUInHundreds": 0,
  "calculatedRuDollarsPerHour": 0,
  "calculatedRuDollarsPerMonth": 0,
  "calculatedEgressPerMonth": 153.255,
  "calculatedStoragePerMonth": 22500,
  "calculatedAnalyticalStoragePerMonth": 600,
  "calculatedTotalPerMonth": 23253.255
}
*/