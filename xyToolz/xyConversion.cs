using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static xyToolz.xyQOL;

namespace xyToolz
{
      public static class xyConversion
      {
            private static string endergebnis = "", endausgabe = "";

            // Wandelt jew Zahl ins gewünschte Zahlensystem um und gibt den Rechenweg aus
            public static string X_to_X(int _aktuelle_Basis, string ausgangszahl, int _basis_des_neuen_Zahlensystems)
            {
                  int _ergebnis = 0, _ergebnis_DEC = 0, _rest, _stelle_von_rechts = 0;
                  string eingabe = DeLetterer(ausgangszahl);
                  int[] _zahlen_ = eingabe.Split(' ')?.Select(Int32.Parse)?.ToArray();

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
                                    Console.WriteLine(Reverse(endergebnis) + " DEC");
                                    break;
                              }
                        case 16:
                              {
                                    Console.WriteLine(Reverse(endergebnis) + " 0xF");
                                    break;
                              }
                        case 2:
                              {
                                    Console.WriteLine(Reverse(endergebnis) + " BIN");
                                    break;
                              }
                        case 8:
                              {
                                    Console.WriteLine(Reverse(endergebnis) + " OKT");
                                    break;
                              }
                  }
                  return endergebnis;
            }

            // entfernt ggf die Buchstaben aus dem String und ersetzt sie durch Zahlen
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
            public static string DEC_to_X(int ausgangszahl, int basis_des_neuen_Zahlensystems)
            {
                  int ergebnis, rest;

                  if (ausgangszahl == 0)
                  {
                        endergebnis += ausgangszahl % basis_des_neuen_Zahlensystems;
                        Console.WriteLine("Ausgangszahl ist nutzlos: " + ausgangszahl);
                        return endergebnis;
                  }
                  else
                  {
                        rest = ausgangszahl % basis_des_neuen_Zahlensystems;
                        ergebnis = ausgangszahl / basis_des_neuen_Zahlensystems;

                        Console.Write(ausgangszahl + " % " + basis_des_neuen_Zahlensystems + " = " + rest + "\t" + "\t");
                        Console.WriteLine(ausgangszahl + " / " + basis_des_neuen_Zahlensystems + " = " + ergebnis);

                        // beide Funktionen machen das gleiche!!! daher nur eine aktivieren 
                        //endergebnis  +=  Letterer (  (ausgangszahl % basis_des_neuen_Zahlensystems).ToString()); 
                        endergebnis = Letterer("" + (ausgangszahl % basis_des_neuen_Zahlensystems));      // KEIN Leerzeichen zwischen den  -->""<-- !!!         

                        ausgangszahl = ergebnis;

                        // passt das Ende der Ausgabe an das gewählte Zahlensystem an
                        switch (basis_des_neuen_Zahlensystems)
                        {
                              case 2:
                                    {
                                          Console.WriteLine(Reverse(endergebnis) + " Bin");
                                          break;
                                    }
                              case 8:
                                    {
                                          Console.WriteLine(Reverse(endergebnis) + " Okt");
                                          break;
                                    }
                              case 12:
                                    {
                                          Console.WriteLine(Reverse(endergebnis) + " DuoDez");
                                          break;
                                    }
                              case 16:
                                    {
                                          Console.WriteLine(Reverse(endergebnis) + " 0xF");
                                          break;
                                    }
                              default:
                                    {
                                          Console.WriteLine(Reverse(endergebnis));
                                          break;
                                    }
                        }
                        return DEC_to_X(ausgangszahl, basis_des_neuen_Zahlensystems);
                  }
            }

            // Hexadezimalzahlen in andere Zahlensysteme konvertieren
            public static string HEX_to_DEC(string ausgangszahl)
            {
                  int ergebnis = 0;
                  string ausgabe = DeLetterer(ausgangszahl);
                  int[] bst = ausgabe.Split(' ')?.Select(Int32.Parse)?.ToArray();

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
            public static string HEX_to_Oct(string ausgangszahl)
            {
                  int temp = int.Parse(HEX_to_DEC(ausgangszahl));
                  string result = DEC_to_X(temp, 8);

                  Console.WriteLine(result);
                  return result;
            }
            public static string HEX_to_Bin(string ausgangszahl)
            {
                  int temp = int.Parse(HEX_to_DEC(ausgangszahl));
                  string result = DEC_to_X(temp, 2);
                  Console.WriteLine(result);
                  return result;
            }

            // Binärzahlen in andere Zahlensysteme konvertieren
            public static string Bin_to_Dec(string ausgangszahl)
            {
                  string ausgabe = DeLetterer(ausgangszahl);
                  int[] zahlen = ausgabe.Split(' ')?.Select(Int32.Parse)?.ToArray();
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
            public static string Bin_to_Hex(string ausgangszahl)
            {
                  int zwischen_Ergebnis = int.Parse(Bin_to_Dec(ausgangszahl));

                  string hex_Zeichen = DEC_to_X(zwischen_Ergebnis, 16);

                  Console.WriteLine(hex_Zeichen);
                  return "" + hex_Zeichen;
            }
            public static string Bin_to_Oct(string ausgangszahl)
            {
                  int temp = int.Parse(Bin_to_Dec(ausgangszahl));
                  string result = DEC_to_X(temp, 8);
                  Console.WriteLine(result);
                  return result;
            }

      }
}
