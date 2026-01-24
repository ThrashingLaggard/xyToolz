using xyToolz.QOL;

namespace xyToolz.Misc
{
        /// <summary>
        /// Hier könnte ihre Werbung stehen!
        /// </summary>
      public static class xyConversion
      {
            private static string endergebnis = "", endausgabe = "";

            // Wandelt jew Zahl ins gewünschte Zahlensystem um und gibt den Rechenweg aus
            /// <summary>
            /// Convert any number into any system
            /// </summary>
            /// <param name="_aktuelle_Basis"></param>
            /// <param name="ausgangszahl"></param>
            /// <param name="_basis_des_neuen_Zahlensystems"></param>
            /// <returns></returns>
            public static string X_to_X(int _aktuelle_Basis, string ausgangszahl, int _basis_des_neuen_Zahlensystems)
            {
                  int _ergebnis = 0, _ergebnis_DEC = 0, _rest, _stelle_von_rechts = 0;
                  string eingabe = DeLetterer(ausgangszahl);
                  int[] _zahlen_ = eingabe.Split(' ')?.Select(Int32.Parse)?.ToArray()!;

                  if (_aktuelle_Basis != 10)
                  {
                        for (int i = _zahlen_.Length - 1; i >= 0; i--)
                        {
                              _zahlen_[i] *= (int)Math.Pow(_aktuelle_Basis, _stelle_von_rechts++);   // der EXPONENT braucht das Inkrement dringend
                        }
                        foreach (int i in _zahlen_)
                        {
                              _ergebnis_DEC += i;
                        }

                        endergebnis += _ergebnis_DEC;
                        Console.WriteLine("Ergebnis in Dezimal: " + _ergebnis_DEC);
                        _aktuelle_Basis = 10;
                  }
                  if (_aktuelle_Basis == 10)
                  {
                        if (_ergebnis_DEC > 0)
                        {
                              goto lol;   // sehr praktisch
                        }
                        else
                        {
                              _ergebnis_DEC = int.Parse(ausgangszahl);
                        }
                  lol:
                        do
                        {
                              _rest = _ergebnis_DEC % _basis_des_neuen_Zahlensystems;
                              _ergebnis = _ergebnis_DEC / _basis_des_neuen_Zahlensystems;

                              // Rechenweg ausgeben
                              Console.Write(_ergebnis_DEC + " % " + _basis_des_neuen_Zahlensystems + " = " + _rest + "\t" + "\t");
                              Console.WriteLine(_ergebnis_DEC + " / " + _basis_des_neuen_Zahlensystems + " = " + _ergebnis);

                              // Lösungs_String formatieren
                              endergebnis = Letterer("" + _ergebnis_DEC % _basis_des_neuen_Zahlensystems);

                              _ergebnis_DEC = _ergebnis;
                        }
                        while (_ergebnis_DEC > 0);
                  }

                  switch (_basis_des_neuen_Zahlensystems)
                  {
                        case 10:
                              {
                                    Console.WriteLine(xy.Reverse(endergebnis) + " DEC");
                                    break;
                              }
                        case 16:
                              {
                                    Console.WriteLine(xy.Reverse(endergebnis) + " 0xF");
                                    break;
                              }
                        case 2:
                              {
                                    Console.WriteLine(xy.Reverse(endergebnis) + " BIN");
                                    break;
                              }
                        case 8:
                              {
                                    Console.WriteLine(xy.Reverse(endergebnis) + " OKT");
                                    break;
                              }
                  }
                  return endergebnis;
            }

            // entfernt ggf die Buchstaben aus dem String und ersetzt sie durch Zahlen
            /// <summary>
            /// Removes letters from the string and replaces them with numbers
            /// </summary>
            /// <param name="number_with_letters"></param>
            /// <returns></returns>
            public static string DeLetterer(string number_with_letters)
            {

                  char[] input = number_with_letters.ToCharArray();
                  char[] input2 = number_with_letters.ToCharArray(); ;
                  int[] high_numbers = { 10, 11, 12, 13, 14, 15 };
                  char[] corresponding_letters = { 'A', 'B', 'C', 'D', 'E', 'F' };
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
                  for (int i = 0; i < digits.Length; i++)
                  {
                        if (input[i].ToString() != digits[i])
                        {
                              input2[i] = input[i];
                        }
                  }

                  string numbers = string.Join(" ", digits);              // String aus Array zusammensetzen

                  Console.WriteLine("Eingegeben: " + new string(input));
                  Console.WriteLine("übersetzt: " + numbers);
                  return numbers;
            }
            //ersetzt alle Zahlen über 9 durch die jeweiligen Buchstaben --> Switch_Case kann Strings ittarieren?!
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

                  endausgabe += too_big_numbers;
                  return endausgabe;
            }



            // Dezimalzahlen in andere Zahlensysteme konvertieren
            /// <summary>
            /// Convert decimal to target system
            /// </summary>
            /// <param name="Number"></param>
            /// <param name="baseOfTargetNumberSystem"></param>
            /// <returns></returns>
            public static string DEC_to_X(int Number, int baseOfTargetNumberSystem)
            {
                  int ergebnis, rest;

                  if (Number == 0)
                  {
                        endergebnis += Number % baseOfTargetNumberSystem;
                        Console.WriteLine("Ausgangszahl ist nutzlos: " + Number);
                        return endergebnis;
                  }
                  else
                  {
                        rest = Number % baseOfTargetNumberSystem;
                        ergebnis = Number / baseOfTargetNumberSystem;

                        Console.Write(Number + " % " + baseOfTargetNumberSystem + " = " + rest + "\t" + "\t");
                        Console.WriteLine(Number + " / " + baseOfTargetNumberSystem + " = " + ergebnis);

                        // beide Funktionen machen das gleiche!!! daher nur eine aktivieren 
                        //endergebnis  +=  Letterer (  (ausgangszahl % basis_des_neuen_Zahlensystems).ToString()); 
                        endergebnis = Letterer("" + (Number % baseOfTargetNumberSystem));      // KEIN Leerzeichen zwischen den  -->""<-- !!!         

                        Number = ergebnis;

                        // passt das Ende der Ausgabe an das gewählte Zahlensystem an
                        switch (baseOfTargetNumberSystem)
                        {
                              case 2:
                                    {
                                          Console.WriteLine(xy.Reverse(endergebnis) + " Bin");
                                          break;
                                    }
                              case 8:
                                    {
                                          Console.WriteLine(xy.Reverse(endergebnis) + " Okt");
                                          break;
                                    }
                              case 12:
                                    {
                                          Console.WriteLine(xy.Reverse(endergebnis) + " DuoDez");
                                          break;
                                    }
                              case 16:
                                    {
                                          Console.WriteLine(xy.Reverse(endergebnis) + " 0xF");
                                          break;
                                    }
                              default:
                                    {
                                          Console.WriteLine(xy.Reverse(endergebnis));
                                          break;
                                    }
                        }
                        return DEC_to_X(Number, baseOfTargetNumberSystem);
                  }
            }

            // Hexadezimalzahlen in andere Zahlensysteme konvertieren
            /// <summary>
            /// Convert Hexa to decimal
            /// </summary>
            /// <param name="number"></param>
            /// <returns></returns>
            public static string HEX_to_DEC(string number)
            {
                  int ergebnis = 0;
                  string ausgabe = DeLetterer(number);
                  int[] bst = ausgabe.Split(' ')?.Select(Int32.Parse)?.ToArray()!;

                  int expo = 0;
                  for (int i = bst.Length - 1; i >= 0; i--)             //zahlen[-1] *= 16^0;
                  {                                                      //zahlen[-2] *= 16^1;
                        bst[i] *= (int)Math.Pow(16, expo++);              //zahlen[-3] *= 16^2;
                  }

                  foreach (int i in bst)
                  {
                        ergebnis += i;
                  }

                  endergebnis += ergebnis;
                  Console.WriteLine("Ergebnis: " + endergebnis);

                  return endergebnis;

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

            // Binärzahlen in andere Zahlensysteme konvertieren
            /// <summary>
            /// Convert binary to decimal
            /// </summary>
            /// <param name="number"></param>
            /// <returns></returns>
            public static string Bin_to_Dec(string number)
            {
                  string ausgabe = DeLetterer(number);
                  int[] zahlen = ausgabe.Split(' ')?.Select(Int32.Parse)?.ToArray()!;
                  int ergebnis = 0;
                  int expo = 0;

                  for (int i = zahlen.Length - 1; i >= 0; i--)            //zahlen[-1] *= 16^0;
                  {                                                       //zahlen[-2] *= 16^1;
                        zahlen[i] *= (int)Math.Pow(2, expo++);              //zahlen[-3] *= 16^2;
                  }

                  foreach (int i in zahlen)
                  {
                        ergebnis += i;
                  }

                  endergebnis += ergebnis;
                  Console.WriteLine("Ergebnis: " + endergebnis);

                  return endergebnis;

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
