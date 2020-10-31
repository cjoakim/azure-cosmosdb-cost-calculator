// Abstract class used to define hard-coded cost and calculation constants.
// Chris Joakim, Microsoft, 2020/10/31

namespace CJoakim.CosmosCalc
{
    public class Constants
    {

        // EGRESS - see https://azure.microsoft.com/en-us/pricing/details/bandwidth/

        public const double EGRESS_TIER_1_MIN_GB = 5.0;
        public const double EGRESS_TIER_1_MAX_GB = 10.0;
        public const double EGRESS_TIER_1_RATE = 0.087;

        public const double EGRESS_TIER_2_MIN_GB = 10.0;
        public const double EGRESS_TIER_2_MAX_GB = 50.0;
        public const double EGRESS_TIER_2_RATE = 0.083;

        public const double EGRESS_TIER_3_MIN_GB = 50.0;
        public const double EGRESS_TIER_3_MAX_GB = 150.0;
        public const double EGRESS_TIER_3_RATE = 0.07;

        public const double EGRESS_TIER_4_MIN_GB = 150.0;
        public const double EGRESS_TIER_4_MAX_GB = 999999999999.0; // Contact Us!
        public const double EGRESS_TIER_4_RATE = 0.05;

    }
}
