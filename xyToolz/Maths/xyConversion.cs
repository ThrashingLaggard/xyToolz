using System;
using System.Linq;
using xyToolz.Extensions;
// ReSharper disable InconsistentNaming
// ReSharper disable TooWideLocalVariableScope
// ReSharper disable UselessBinaryOperation

namespace xyToolz.Maths
{
        /// <summary>
        /// This class is used to convert numbers between different systems, like hex -> bin
        /// </summary>
      public static class xyConversion
      {
            /// <summary>
            /// Convert any number into any system
            /// </summary>
            /// <param name="currentBase">Base of the current number system</param>
            /// <param name="startNumber">The number to go from</param>
            /// <param name="newBase">The base of the target number system</param>
            /// <returns></returns>
            public static string X_to_X(int currentBase, string startNumber, int newBase)
            {
                  int _ergebnis_DEC, _stelle_von_rechts = 0;

                  if (currentBase != 10)
                  {
                        string eingabe = DeLetterer(startNumber);
                        int[] _zahlen_ = eingabe.Split(' ').Select(int.Parse).ToArray();

                        for (int i = _zahlen_.Length - 1; i >= 0; i--)
                        {
                              _zahlen_[i] *= (int)Math.Pow(currentBase, _stelle_von_rechts++);   // der EXPONENT braucht das Inkrement dringend
                        }

                        _ergebnis_DEC = _zahlen_.Sum();
                        Console.WriteLine("Ergebnis in Dezimal: " + _ergebnis_DEC);
                  }
                  else
                  {
                        _ergebnis_DEC = int.Parse(startNumber);
                  }

                  // Rechenweg und Ergebnis gibt DEC_to_X aus
                  return DEC_to_X(_ergebnis_DEC, newBase);
            }

            /// <summary>
            /// Removes letters from the string and replaces them with numbers
            /// </summary>
            /// <param name="number_with_letters"></param>
            /// <returns>string numberWithoutLetters</returns>
            public static string DeLetterer(string number_with_letters)
            {
                  number_with_letters = number_with_letters.ToUpperInvariant();
                  char[] input = number_with_letters.ToCharArray();
                  //char[] input2 =[];
                  int[] high_numbers = [10, 11, 12, 13, 14, 15];
                  char[] corresponding_letters = ['A', 'B', 'C', 'D', 'E', 'F'];
                  string[] digits = new string[number_with_letters.Length];

                  for (int i = 0; i < number_with_letters.Length; i++)
                  {
                        digits[i] = number_with_letters[i].ToString();
                  }

                  for (int i = 0; i < input.Length; i++)
                  {
                        for (int j = 0; j < high_numbers.Length; j++)
                        {
                              if (input[i] == corresponding_letters[j])
                              {
                                    digits[i] = high_numbers[j].ToString();
                                    break;
                              }
                        }
                  }
                  //for (int i = 0; i < digits.Length; i++)
                  //{
                  //      if (input[i].ToString() != digits[i])
                  //      {
                  //            input2[i] = input[i];
                  //      }
                  //}

                  string numbers = string.Join(" ", digits);              

                  Console.WriteLine("Eingegeben: " + new string(input));
                  Console.WriteLine("übersetzt: " + numbers);
                  return numbers;
            }

            /// <summary>
            /// Switches all the numbers > 9 with letters
            /// </summary>
            /// <param name="too_big_numbers"></param>
            /// <returns></returns>
            public static string Letterer(string too_big_numbers)
            {
                  switch (too_big_numbers)
                  {
                        case "10":
                              {
                                    too_big_numbers = "A";
                                    break;
                              }
                        case "11":
                              {
                                    too_big_numbers = "B";
                                    break;
                              }
                        case "12":
                              {
                                    too_big_numbers = "C";
                                    break;
                              }
                        case "13":
                              {
                                    too_big_numbers = "D";
                                    break;
                              }
                        case "14":
                              {
                                    too_big_numbers = "E";
                                    break;
                              }
                        case "15":
                              {
                                    too_big_numbers = "F";
                                    break;
                              }
                  }

                  return too_big_numbers;
            }



            /// <summary>
            /// Convert decimal to target system
            /// </summary>
            /// <param name="Number"></param>
            /// <param name="baseOfTargetNumberSystem"></param>
            /// <returns></returns>
            public static string DEC_to_X(int Number, int baseOfTargetNumberSystem)
            {
                  int ergebnis, rest;
                  string endergebnis = "";

                  if (baseOfTargetNumberSystem < 2 || baseOfTargetNumberSystem > 16)
                  {
                        throw new ArgumentOutOfRangeException(nameof(baseOfTargetNumberSystem), baseOfTargetNumberSystem, "Base must be between 2 and 16.");
                  }

                  if (Number == 0)
                  {
                        Console.WriteLine("Ausgangszahl ist nutzlos: " + Number);
                        return "0";
                  }

                  while (Number > 0)
                  {
                        rest = Number % baseOfTargetNumberSystem;
                        ergebnis = Number / baseOfTargetNumberSystem;

                        Console.Write(Number + " % " + baseOfTargetNumberSystem + " = " + rest + "\t" + "\t");
                        Console.WriteLine(Number + " / " + baseOfTargetNumberSystem + " = " + ergebnis);

                        // Neue Ziffer kommt nach vorne, die Reste fallen von rechts nach links an
                        endergebnis = Letterer("" + rest) + endergebnis;      // KEIN Leerzeichen zwischen den  -->""<-- !!!

                        Number = ergebnis;
                  }

                  switch (baseOfTargetNumberSystem)
                  {
                        case 2:
                              {
                                    Console.WriteLine(endergebnis + " Bin");
                                    break;
                              }
                        case 8:
                              {
                                    Console.WriteLine(endergebnis + " Okt");
                                    break;
                              }
                        case 10:
                              {
                                    Console.WriteLine(endergebnis + " Dez");
                                    break;
                              }
                        case 12:
                              {
                                    Console.WriteLine(endergebnis + " DuoDez");
                                    break;
                              }
                        case 16:
                              {
                                    Console.WriteLine(endergebnis + " 0xF");
                                    break;
                              }
                        default:
                              {
                                    Console.WriteLine(endergebnis);
                                    break;
                              }
                  }
                  return endergebnis;
            }

            /// <summary>
            /// Convert Hexa to decimal
            /// </summary>
            /// <param name="number"></param>
            /// <returns></returns>
            public static string HEX_to_DEC(string number)
            {
                  int ergebnis = 0;
                  string ausgabe = DeLetterer(number);
                  int[] bst = ausgabe.Split(' ').Select(int.Parse).ToArray();

                  int expo = 0;
                  for (int i = bst.Length - 1; i >= 0; i--)             
                  {
                        bst[i] *= (int)Math.Pow(16, expo++);              
                  }

                  foreach (int i in bst)
                  {
                        ergebnis += i;
                  }

                  Console.WriteLine("Ergebnis: " + ergebnis);

                  return ergebnis.ToString();

            }
            /// <summary>
            /// Convert Hexa to octal
            /// </summary>
            /// <param name="number"></param>
            /// <returns></returns>
            public static string HEX_to_Oct(string number)
            {
                  int temp = int.Parse(HEX_to_DEC(number));
                  string result = DEC_to_X(temp, 8);

                  Console.WriteLine(result);
                  return result;
            }
            /// <summary>
            /// Convert Hexa to binary
            /// </summary>
            /// <param name="number"></param>
            /// <returns></returns>
            public static string HEX_to_Bin(string number)
            {
                  int temp = int.Parse(HEX_to_DEC(number));
                  string result = DEC_to_X(temp, 2);
                  Console.WriteLine(result);
                  return result;
            }

            /// <summary>
            /// Convert binary to decimal
            /// </summary>
            /// <param name="number"></param>
            /// <returns></returns>
            public static string Bin_to_Dec(string number)
            {
                  string ausgabe = DeLetterer(number);
                  int[] zahlen = ausgabe.Split(' ').Select(int.Parse).ToArray();
                  int ergebnis = 0;
                  int expo = 0;

                  for (int i = zahlen.Length - 1; i >= 0; i--)            
                  {                                                       
                        zahlen[i] *= (int)Math.Pow(2, expo++);              
                  }

                  foreach (int i in zahlen)
                  {
                        ergebnis += i;
                  }

                  Console.WriteLine("Ergebnis: " + ergebnis);

                  return ergebnis.ToString();

            }
            /// <summary>
            /// Convert binary to hexadecimal
            /// </summary>
            /// <param name="number"></param>
            /// <returns></returns>
            public static string Bin_to_Hex(string number)
            {
                  int zwischen_Ergebnis = int.Parse(Bin_to_Dec(number));

                  string hex_Zeichen = DEC_to_X(zwischen_Ergebnis, 16);

                  Console.WriteLine(hex_Zeichen);
                  return "" + hex_Zeichen;
            }
            /// <summary>
            /// Convert binary to octal
            /// </summary>
            /// <param name="number"></param>
            /// <returns></returns>
            public static string Bin_to_Oct(string number)
            {
                  int temp = int.Parse(Bin_to_Dec(number));
                  string result = DEC_to_X(temp, 8);
                  Console.WriteLine(result);
                  return result;
            }

      }
}
