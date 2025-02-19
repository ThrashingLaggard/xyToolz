using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using con_Logger.Loggers;

namespace xyToolz
{
      public static class xyQOL
      {
            public static string Repeat(string what_to_repeat, ushort quantity)
            {
                  StringBuilder stringBuilder = new();
                  for (int i = 0; i < quantity; i++)
                  {
                        stringBuilder.Append(what_to_repeat);
                  }
                  return stringBuilder.ToString();
            }
            public static string Reverse(string old_order)
            {

                  char[] cache = old_order.ToCharArray();

                  Array.Reverse(cache);
                  string neues_Ergebnis = new string(cache);

                  return neues_Ergebnis;
            }
            public static void Print(string what_to_print)
            {
                  sLog.Log(what_to_print);
            }


            public static void EDITOR()
            {
                  Process.Start("notepad.exe");
            }
            public static void EDITOR(string filepath)
            {
                  Process.Start("notepad.exe", filepath);
            }

            public static string crash(int high_number)
            {
                  int a = 0;
                  int b = 0;
                  string lol = "";

                  if (high_number == 0)
                  {
                        Console.WriteLine("NEIN");
                        return lol;
                  }
                  else
                  {
                        high_number -= 8;
                        Console.WriteLine(high_number);
                        if (high_number < 88)
                              goto lol;

                        return crash(high_number);

                  lol:
                        {
                              Console.WriteLine("Hey");
                              a++;
                              if (b > (high_number / 2))
                              {
                                    Console.WriteLine(a);
                              }
                              goto rofl;
                        }

                  rofl:
                        {
                              Console.WriteLine("Ho");
                              if (a > (high_number / 2))
                              {
                                    Console.WriteLine(b);
                              }
                              ++b;
                              goto lol;
                        }
                  }
            }
      }
}
