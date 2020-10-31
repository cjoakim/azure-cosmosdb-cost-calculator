using System;
using System.Collections.Generic;

// Class Container represents a Container in CosmosDB for the purpose
// of calculating costs.
//
// Chris Joakim, Microsoft, 2020/10/31

namespace CJoakim.CosmosCalc
{
    public class Container
    {
        // Constants:
        public const string PROVISIONING_TYPE_STANDARD     = "standard";
        public const string PROVISIONING_TYPE_AUTOSCALE    = "autoscale";
        public const string PROVISIONING_TYPE_SERVERLESS   = "serverless";
        public const string REPLICATION_TYPE_SINGLE_REGION = "single";
        public const string REPLICATION_TYPE_MULTI_REGION  = "multi-region";
        public const string REPLICATION_TYPE_MULTI_MASTER  = "multi-master";
        public const int    ABSOLUTE_MIN_THROUGHPUT        = 400;

        public const double WEEKS_PER_MONTH = 52.0 / 12.0;
        public const double HOURS_PER_WEEK  = (24.0 * 7.0);
        public const double HOURS_PER_MONTH = WEEKS_PER_MONTH * HOURS_PER_WEEK;

        // Instance variables:
        public  string name     { get; set; }
        public  double sizeInGB { get; set; }
        public  string provisioningType { get; set; }
        public  bool   availabilityZone { get; set; }
        public  bool   synapseLinkEnabled { get; set; }
        public  string replicationType  { get; set; }
        public  int    regionCount      { get; set; }
        public  int    ruPerSecond      { get; set; }
        public  int    maxHistoricalManualRu { get; set; }
        public  int    maxHistoricalAutoRu   { get; set; }
        public  double replicatedGBPerMonth { get; set; }
        

        // The above fields are set per the input text file,
        // while the following fields are calculated
        public  int    calculatedMinRU             { get; set; }
        public  double calculatedRatePer100RU      { get; set; }
        public  double calculatedRUInHundreds      { get; set; }
        public  double calculatedRuDollarsPerHour  { get; set; }
        public  double calculatedRuDollarsPerMonth { get; set; }
        public  double calculatedEgressPerMonth    { get; set; }
        public  double calculatedStoragePerMonth   { get; set; }
        public  double calculatedAnalyticalStoragePerMonth { get; set; }
        public  double calculatedTotalPerMonth     { get; set; }

        public Container()
        {
            this.name = null;
            this.sizeInGB = 0.0;
            this.provisioningType = PROVISIONING_TYPE_STANDARD;
            this.availabilityZone = false;
            this.replicationType  = REPLICATION_TYPE_SINGLE_REGION;
            this.regionCount      = 1;
            this.ruPerSecond      = 0;
            this.calculatedMinRU  = -1;
            this.calculatedRatePer100RU = 0;
            this.replicatedGBPerMonth = 0.0;
        }

        public void SetProvisioningType(string type)
        {
            switch (type) {             
                case "standard": 
                    this.provisioningType = PROVISIONING_TYPE_STANDARD;
                    break; 
                case "autoscale": 
                    this.provisioningType = PROVISIONING_TYPE_AUTOSCALE;
                    break; 
                case "serverless": 
                    Console.WriteLine("ProvisioningType 'serverless' is not yet supported"); 
                    break; 
                default: 
                    Console.WriteLine("Unknown value in SetProvisioningType: " + type); 
                    break; 
            }
        }

        public void SetReplicationType(string type)
        {
            switch (type) {             
                case "single": 
                    this.replicationType = REPLICATION_TYPE_SINGLE_REGION;
                    break; 
                case "multi-region": 
                    this.replicationType = REPLICATION_TYPE_MULTI_REGION;
                    break; 
                case "multi-master": 
                    this.replicationType = REPLICATION_TYPE_MULTI_MASTER;
                    break; 
                default: 
                    Console.WriteLine("Unknown value in SetReplicationType: " + type); 
                    break; 
            }
        }

        public void SetSizeInBytes(double n)
        {
            this.sizeInGB = n / Math.Pow(1024, 3);
        }

        public void SetSizeInMB(double n)
        {
            this.sizeInGB = n / 1024.0;
        }

        public void SetSizeInTB(double n)
        {
            this.sizeInGB = n * 1024.0;
        }

        public void SetSizeInPB(double n)
        {
            this.sizeInGB = n * 1024.0 * 1024.0;
        }

        public int CalculateMinRU()
        {
            int min1 = ABSOLUTE_MIN_THROUGHPUT;
            int min2 = MinRuBasedOnGB();
            int min3 = MinRuBasedOnManualProvisioning();
            int min4 = MinRuBasedOnAutoProvisioning();

            List<int> minimums = new List<int>() {min1, min2, min3, min4};
            minimums.Sort();
            calculatedMinRU = RoundUpToHundreds(minimums[minimums.Count - 1]);
            return calculatedMinRU;
        }

        public int RoundUpToHundreds(int ru) 
        {
            double fraction = ((double) ru) / 100.0;
            return (int) Math.Round(fraction, 0, MidpointRounding.ToPositiveInfinity) * 100;
        }

        private int MinRuBasedOnGB()
        {
            return (int) sizeInGB * 10;
        }

        private int MinRuBasedOnManualProvisioning()
        {
            if (maxHistoricalManualRu > 0)
            {
                return (int) (maxHistoricalManualRu / 100);
            }
            else
            {
                return 0;
            }
        }

        private int MinRuBasedOnAutoProvisioning()
        {
            if (maxHistoricalAutoRu > 0)
            {
                return (int)(maxHistoricalAutoRu / 10);
            }
            else
            {
                return 0;
            }
        }

        public double CalculateCosts()
        {
            CalculateMinRU();
            CalculateHourlyRatePer100RU();
            CalculateEgress();

            calculatedRUInHundreds = ruPerSecond / 100.0;

            calculatedRuDollarsPerHour =
                calculatedRUInHundreds * calculatedRatePer100RU;

            calculatedRuDollarsPerMonth = 
                calculatedRuDollarsPerHour * HOURS_PER_MONTH;

            calculatedStoragePerMonth = (sizeInGB * 0.25) * ((double) regionCount);

            if (synapseLinkEnabled)
            {
                calculatedAnalyticalStoragePerMonth = (sizeInGB * 0.02);
            }

            calculatedTotalPerMonth =
                calculatedRuDollarsPerMonth +
                calculatedStoragePerMonth +
                calculatedEgressPerMonth +
                calculatedAnalyticalStoragePerMonth;

            return calculatedTotalPerMonth;
        }

        public double CalculateHourlyRatePer100RU() 
        {
            calculatedRatePer100RU = 0.008;

            if (provisioningType == PROVISIONING_TYPE_AUTOSCALE)
            {
                if (replicationType == REPLICATION_TYPE_SINGLE_REGION)
                {
                    calculatedRatePer100RU = (double) 0.012;
                    if (availabilityZone)
                    {
                        calculatedRatePer100RU = (double) calculatedRatePer100RU * 1.25;
                    }
                }

                if (replicationType == REPLICATION_TYPE_MULTI_REGION)
                {
                    calculatedRatePer100RU = 0.012;
                }

                if (replicationType == REPLICATION_TYPE_MULTI_MASTER)
                {
                    calculatedRatePer100RU = 0.016;
                }
            }

            if (provisioningType == PROVISIONING_TYPE_STANDARD)
            {
                if (replicationType == REPLICATION_TYPE_SINGLE_REGION)
                {
                    calculatedRatePer100RU = 0.008 ;
                    if (availabilityZone)
                    {
                        calculatedRatePer100RU = calculatedRatePer100RU * 1.25;
                    }
                }

                if (replicationType == REPLICATION_TYPE_MULTI_REGION)
                {
                    calculatedRatePer100RU = 0.012;
                }

                if (replicationType == REPLICATION_TYPE_MULTI_MASTER)
                {
                    calculatedRatePer100RU = 0.016;
                }
            }

            if (provisioningType == PROVISIONING_TYPE_SERVERLESS)
            {
                throw new Exception("provisioningType " + PROVISIONING_TYPE_SERVERLESS + " is not yet supported by this calculator");
            }

            return calculatedRatePer100RU;
        }

        public double CalculateEgress()
        {
            calculatedEgressPerMonth = 0.0;

            if (regionCount > 1)
            {
                if (replicatedGBPerMonth > Constants.EGRESS_TIER_1_MIN_GB)
                {
                    double tier1GB = GBInEgressTier(Constants.EGRESS_TIER_1_MIN_GB, Constants.EGRESS_TIER_1_MAX_GB);
                    double tier2GB = GBInEgressTier(Constants.EGRESS_TIER_2_MIN_GB, Constants.EGRESS_TIER_2_MAX_GB);
                    double tier3GB = GBInEgressTier(Constants.EGRESS_TIER_3_MIN_GB, Constants.EGRESS_TIER_3_MAX_GB);
                    double tier4GB = GBInEgressTier(Constants.EGRESS_TIER_4_MIN_GB, Constants.EGRESS_TIER_4_MAX_GB);

                    //Console.WriteLine("tier1GB: " + tier1GB);
                    //Console.WriteLine("tier2GB: " + tier2GB);
                    //Console.WriteLine("tier3GB: " + tier3GB);
                    //Console.WriteLine("tier4GB: " + tier4GB);

                    calculatedEgressPerMonth =
                        tier1GB * Constants.EGRESS_TIER_1_RATE +
                        tier2GB * Constants.EGRESS_TIER_2_RATE +
                        tier3GB * Constants.EGRESS_TIER_3_RATE +
                        tier4GB * Constants.EGRESS_TIER_4_RATE;
                }
            }
            return calculatedEgressPerMonth * ((double) regionCount - 1);
        }

        private double GBInEgressTier(double tierMin, double tierMax)
        {
            double gb = 0;
            double tierRange = tierMax - tierMin;

            if (replicatedGBPerMonth > tierMin)
            {
                gb = replicatedGBPerMonth - tierMin;
                if (gb > tierRange)
                {
                    gb = tierRange;
                }
            }
            return gb;
        }
    }
}
