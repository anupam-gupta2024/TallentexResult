namespace TallentexResult.Servies
{
    public class Performance
    {
        // represents the keyed value
        private struct Interval
        {
            public decimal Min;
            public decimal Max;
        }

        /// <summary>
        /// list of achievement based on marks range
        /// </summary>
        private static readonly IDictionary<Interval, string> _dictionaryAchievements
            = new Dictionary<Interval, string> {
                { new Interval { Min = -100M, Max = 29.99M }, "Not Achieved" },
                { new Interval { Min = 30M, Max = 39.99M }, "Elementary Achievement" },
                { new Interval { Min = 40M, Max = 49.99M }, "Moderate Achievement" },
                { new Interval { Min = 50M, Max = 59.99M }, "Adequate Achievement" },
                { new Interval { Min = 60M, Max = 69.99M }, "Substantial Achievement" },
                { new Interval { Min = 70M, Max = 79.99M }, "Meritorious Achievement" },
                { new Interval { Min = 80M, Max = 100M }, "Outstanding Achievement" }
            };

        /// <summary>
        /// list of color based on marks range
        /// </summary>
        private static readonly IDictionary<Interval, string> _dictionaryColors
            = new Dictionary<Interval, string> {
                { new Interval { Min = -100M, Max = 29.99M }, "danger" },
                { new Interval { Min = 30M, Max = 49.99M }, "warning" },
                { new Interval { Min = 30M, Max = 79.99M }, "primary" },
                { new Interval { Min = 80M, Max = 100M }, "success" }
            };

        /// <summary>
        /// get achievement by mark
        /// </summary>
        public static string getAchievement(decimal value)
        {
            foreach (var d in _dictionaryAchievements)
            {
                Interval interval = d.Key;
                if (value >= interval.Min && value <= interval.Max)
                    return d.Value;
            }

            return "";
        }

        /// <summary>
        /// get color by mark
        /// </summary>
        public static string getColor(decimal value)
        {
            foreach (var d in _dictionaryColors)
            {
                Interval interval = d.Key;
                if (value >= interval.Min && value <= interval.Max)
                    return d.Value;
            }

            return "";
        }
    }
}
