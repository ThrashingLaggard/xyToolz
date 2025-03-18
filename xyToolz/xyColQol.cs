using System.Collections;


namespace xyToolz
{
    /// <summary>
    /// Helpers for Enumerables:
    /// 
    /// Fill with integer
    ///             -> with even
    ///             -> with odd
    ///             
    /// Print whole thingy
    ///         --> also with Carriage return
    /// 
    /// Fill with a time for every quarter hour of the day 
    ///     
    /// </summary>
    public static class xyColQol
    {
        public static IEnumerable<int> FillTheList(int limit)
        {
            var list = new List<int>();

            for (int i = 0; i < limit; i++)
            {
                list.Add(i);
            }
            return list;
        }

        public static IEnumerable<int> FillEvenList(int limit)
        {
            var lst_EvenNumbers = new List<int>();

            for (int i = 2; i < limit; i += 2)
            {
                lst_EvenNumbers.Add(i);
            }
            return lst_EvenNumbers;
        }

        public static IEnumerable<int> FillOddList(int limit)
        {
            var lst_OddNumbers = new List<int>();

            for (int i = 1; i < limit; i += 2)
            {
                lst_OddNumbers.Add(i);
            }
            return lst_OddNumbers;
        }

        /// <summary>
        /// Print the targets intestines on your favourite console
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string Spill(IEnumerable values)
        {
            string output = string.Empty;
            foreach (object value in values)
            {
                output += value + ", ";
            }
            xyLog.Log(output);
            return output;
        }


        /// <summary>
        /// Print the targets intestines on your favourite console BUT ASYNC
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static async Task<string> AsxSpill(IEnumerable values)
        {
            string output = string.Empty;
            await Task.Run
            (
                () =>
                {
                    foreach (object value in values)
                    {
                        output += value + ", ";
                    }
                }
            );

            await xyLog.AsxLog(output);
            return output;
        }

        /// <summary>
        /// Print the targets intestines on your favourite console with CARRIAGE RETURNS after every value
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static async Task<string> AsxSpillDown(IEnumerable values)
        {
             string output = string.Empty;
             await Task.Run
             (
                 async () =>
                 {
                     foreach (object value in values)
                     {
                         output += value + ", ";
                         await xyLog.AsxLog(value + "");
                     }
                 }
             );

            return output;
        }


        /// <summary>
        /// Print the targets intestines on your favourite console with CARRIAGE RETURNS after every value
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string SpillDown(IEnumerable values)
        {
            string output = string.Empty;
            foreach (object value in values)
            {
                output += value + ", ";
                xyLog.Log(value + "");
            }
            return output;
        }

        /// <summary>
        /// Returns a list filled with all the quarters of an hour in the day
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<string> GetQuarterHours()
        {
            var presets = new List<string>();
            for (int i = 0; i < 24; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    // Wert in der Schleife aktualisieren!
                    int minutes = 15 * (j + 1);
                    int hour = i;

                    // Stunden inkrementieren
                    if (minutes == 60)
                    {
                        minutes = 0;
                        hour = (i + 1) % 24;
                    }

                    // Mit führenden Nullen formatieren
                    string str_Time = $"{hour:D2}:{minutes:D2}";

                    // Zur Liste hinzufügen und dann verwerfen
                    presets.Add(str_Time);
                    str_Time = string.Empty;
                }
            }
            return presets;
        }

    }
}
