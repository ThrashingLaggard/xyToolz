using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections;


namespace xyToolz
{
    /// <summary>
    /// Little helpers:
    /// 
    /// Repeat  or  reverse   strings
    /// ---
    /// Open Editor 
    ///         -> also with file
    ///         ---------------------------
    ///         
    /// "Useless" experiments:
    /// 
    /// Biep
    /// ---
    /// Crash
    /// 
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Benennungsstile", Justification = "Because XyQol and XYQOL look like shit, and I dont have a better naming idea for my libs.")]
    public static class xyQOL
    {

        //public static async  object TryCatch(Func<object,object>dangerousMethod, object param)
        //{
        //      try
        //      {
        //            return dangerousMethod(param);
        //      }
        //      catch(Exception ex)
        //      {
        //            xyLog.ExLog(ex);
        //      }
        //}



        /// <summary>
        /// Repeat the string the given number of times
        /// </summary>
        /// <param name="what_to_repeat"></param>
        /// <param name="quantity"></param>
        /// <returns>  (+++ , 2 )  -->   +++ +++ +++ </returns>
        public static string Repeat(string what_to_repeat, ushort quantity)
        {
            StringBuilder stringBuilder = new();
            for (int i = 0; i < quantity; i++)
            {
                stringBuilder.Append(what_to_repeat);
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Reverse the target String
        /// </summary>
        /// <param name="old_order"></param>
        /// <returns></returns>
        public static string Reverse(string old_order)
        {
            char[] cache = old_order.ToCharArray();

            Array.Reverse(cache);
            string neues_Ergebnis = new string(cache);

            return neues_Ergebnis;
        }

        /// <summary>
        /// Write on Console, trigger evts
        /// </summary>
        /// <param name="what_to_print"></param>
        public static void Print(string what_to_print)
        {
            xyLog.Log(what_to_print);
        }


        /// <summary>
        /// Get a byte array from  utf8 string
        /// </summary>
        /// <param name="target"></param>
        /// <returns>byte[]</returns>
            public static byte[] StringBytes( string target )
            {
                  try
                  {
                        return Encoding.UTF8.GetBytes(target);
                  }
                  catch (Exception ex)
                  {
                        xyLog.ExLog(ex);
                        return null!;
                  }
            }


            /// <summary>
            /// Open Notepad.exe
            /// </summary>
            public static void EDITOR()
        {
            Process.Start("notepad.exe");
        }

        /// <summary>
        /// Open Notepad.exe with the specified file
        /// </summary>
        public static void EDITOR(string filepath)
        {
            Process.Start("notepad.exe", filepath);
        }

        #region "Experiments an Research Chemicals"

        /// <summary>
        /// Bei dir piepts wohl!
        /// </summary>
        public static void Piep()
        {
            Console.Beep();
            String Beep = xyLog.Log("Beep!");
        }

        /// <summary>
        /// Wenn dein Pc schnell genug ist, stürzt dieses Progamm nicht ab.
        /// </summary>
        /// <param name="high_number"></param>
        /// <returns></returns>
        public static string Crash(UInt128 high_number)
        {
            UInt128 a = 0;
            UInt128 b = 0;
            string lol = "";

            if (high_number == 0)
            {
                Console.WriteLine("NEIN");
                return lol;
            }
            else
            {
                high_number -= 88;
                Console.WriteLine(high_number);
                if (high_number < 8888)
                    goto lol;

                return Crash(high_number);

            lol:
                {
                    Console.WriteLine("Hey");
                    a++;
                    if (b > (high_number / 2))
                    {
                        Piep();
                        Console.WriteLine(a);
                    }
                    goto rofl;
                }

            rofl:
                {
                    Console.WriteLine("Ho");
                    if (a > (high_number / 2))
                    {
                        Piep();
                        Console.WriteLine(b);
                    }
                    ++b;
                    goto lol;
                }
            }
        }

        #endregion

    }
}
