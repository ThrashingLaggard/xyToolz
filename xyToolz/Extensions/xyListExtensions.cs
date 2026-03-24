using System.Collections;
using System.Linq.Expressions;
using System.Text;
using xyToolz.Helper.Logging;

namespace xyToolz.Extensions
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
      public static class xyListExtensions
      {
            public static IList<int> FillWithNumbers( this IList<int> list, int limit )
            {
                  for (int i = 0; i <= limit; i++)
                  {
                        list.Add(i);
                  }
                  return list;
            }

            public static IList<int> AddEvenNumbers(this IList<int>list, int limit )
            {
                  for (int i = 2; i <= limit; i += 2)
                  {
                        list.Add(i);
                  }
                  return list;
            }

            public static IList<int> AddOddNumbers( this IList<int> list, int limit )
            { 
                for (int i = 1; i < limit; i += 2)
                  {
                        list.Add(i);
                  }
                  return list;
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
                              int minutes = 15 * ( j + 1 );
                              int hour = i;

                              // Stunden inkrementieren
                              if (minutes == 60)
                              {
                                    minutes = 0;
                                    hour = ( i + 1 ) % 24;
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
