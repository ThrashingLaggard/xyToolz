using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using xyToolz.Helper.Logging;

namespace xyToolz.PDF
{
    /// <summary>
    /// QOL stuff for handling pdf files
    /// </summary>
    public class xyPdf
    {
        // Es heißt WORKING-DÍRECTORY

        /// <summary>
        /// creates an internal PDF_Document from the specified file  -  requires said file to be .pdf
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static PdfDocument OpenDoc(string filepath)
        {
            PdfDocument new_File = new();
            if (!File.Exists(filepath))
            {
                throw new FileNotFoundException($"File not found: {filepath}");
            }
            try
            {
                if (PdfReader.TestPdfFile(filepath) > 0)
                {
                    new_File = PdfReader.Open(filepath);
                    new_File.Info.Title = filepath;
                }
            }
            catch (Exception e)
            {
                xyLog.ExLog(e);
            }
            return new_File;
        }

        /// <summary>
        /// Saves the specified picture as a .pdf File after converting it with the according method.
        /// 
        /// 
        /// returns a bool for now
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="newpath"></param>
        /// <returns>bool</returns>
        public static bool SaveThisPicAsPdf(string filepath, string newpath)
        {
            try
            {
#pragma warning disable CS0612 // Typ oder Element ist veraltet
                ConvertPictureToPdf(filepath).Save($"{newpath}.pdf");
#pragma warning restore CS0612 // Typ oder Element ist veraltet
                return true;
            }
            catch (Exception e)
            {
                xyLog.ExLog(e);
                return false;
            }
        }

        /// <summary>
        /// Copies pages from a source document to a target document and appends them.
        /// </summary>
        private static void CopyPagesToTarget(PdfDocument sourceDoc, PdfDocument targetDoc)
        {
            foreach (PdfPage page in sourceDoc.Pages)
            {
                targetDoc.AddPage(page);
            }
        }

        /// <summary>
        /// For ErrorPrevention gives the Files an info.title
        /// </summary>
        /// <param name="pdfs"></param>
        /// <returns></returns>
        public static bool NameAllFiles(List<PdfDocument> pdfs)
        {
            double x = 1.0;
            try
            {
                foreach (PdfDocument test in pdfs)
                {
                    if (test.Info.Title == string.Empty)
                    {
                        // Why is this here???
                        if (x - (int)x == 0)
                        {
                            test.Info.Title = $"{x}";
                            test.Comment = $"{x}";
                            x += 0.5;
                        }
                        else
                        {

                            test.Info.Title = $"{x}";
                            test.Comment = $"{x}";
                            x += 0.5;
                        }
                    }
                }
                return true;
            }
            catch
            {
                Console.WriteLine("Couldnt give every file a title, but at least we got " + x);
                return false;
            }

        }



        public static PdfDocument CombineTwoPDF(PdfDocument first, PdfDocument second)
        {
            PdfDocument firstDoc = PdfReader.Open(first.FullPath, PdfDocumentOpenMode.Import);
            PdfDocument secondDoc = PdfReader.Open(second.FullPath, PdfDocumentOpenMode.Import);

            PdfDocument targetDoc = new PdfDocument();

            foreach (PdfPage page in first.Pages)
            {
                targetDoc.AddPage(page);
            }
            foreach (PdfPage page in second.Pages)
            {
                targetDoc.AddPage(page);
            }
            string outputDirectory = "Output";
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            targetDoc.Save(Path.Combine(outputDirectory, $"{targetDoc.PageCount}.pdf"));
            PdfDocument pdf = targetDoc;
            targetDoc.Close();
            return pdf;
        }


        /// <summary>
        /// Combine two PDF files and save their page count as name
        /// </summary>
        /// <returns></returns>
        public static PdfDocument CombineTwoPDFs(PdfDocument first, PdfDocument second)
        {
            PdfDocument targetDoc = new PdfDocument();
            PdfDocument firstDoc = PdfReader.Open(first.FullPath, PdfDocumentOpenMode.Import);
            PdfDocument secondDoc = PdfReader.Open(second.FullPath, PdfDocumentOpenMode.Import);

            foreach (PdfPage page in first.Pages)
            {
                targetDoc.AddPage(page);
            }
            foreach (PdfPage page in second.Pages)
            {
                targetDoc.AddPage(page);
            }

            string outputDirectory = "Output";
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            //targetDoc.Save(Path.Combine(outputDirectory, $"{targetDoc.PageCount}.pdf"));
            PdfDocument pdf = targetDoc;
            targetDoc.Close();
            return pdf;
        }

        /// <summary>
        /// Takes ONE picture and converts it into an internal instance of a PdfSharp PdfDocument
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns>PdfDocument</returns>
        [Obsolete]
        public static PdfDocument ConvertPictureToPdf(string filepath)
        {
            PdfPage fromPic = new PdfPage();
            PdfDocument document = new PdfDocument();
            document.AddPage(fromPic);
            fromPic.Orientation = PageOrientation.Landscape;

            XImage xImage = XImage.FromFile(filepath);

            // Calculate the scaling factor to fit the image on the page while preserving the aspect ratio
            double imageRatio = (double)xImage.PixelWidth / xImage.PixelHeight;
            double pageRatio = (double)fromPic.Width / fromPic.Height;

            double width, height;

            // Determine dimensions to fit image within page
            if (imageRatio > pageRatio)
            {
                // Image is wider than page aspect ratio; fit width
                width = fromPic.Width;
                height = fromPic.Width / imageRatio;
            }
            else
            {
                // Image is taller than page aspect ratio; fit height
                height = fromPic.Height;
                width = fromPic.Height * imageRatio;
            }

            // Calculate position to center the image on the page
            double xPosition = (fromPic.Width - width) / 2;
            double yPosition = (fromPic.Height - height) / 2;

            XGraphics graphicsElement = XGraphics.FromPdfPage(fromPic);
            XRect rect = new XRect(xPosition, yPosition, width, height);
            graphicsElement.DrawImage(xImage, rect);
            //document.Info.Title = filepath.Replace(Path.GetExtension(filepath), ".pdf"); // Set output path as .pdf
            return document;
        }

        /// <summary>
        /// Two pics combined in one doc, but without saving it as file
        /// </summary>
        /// <param name="first_path"></param>
        /// <param name="other_path"></param>
        /// <returns></returns>
        [Obsolete]
        public static PdfDocument ConvertPictureToPdf(string first_path, string other_path)
        {
            PdfDocument document = new PdfDocument();
            PdfPage firstPic = new PdfPage(document);
            PdfPage secondPic = new PdfPage(document);

            firstPic.Orientation = PageOrientation.Landscape;
            secondPic.Orientation = firstPic.Orientation;

            XImage xImage = XImage.FromFile(first_path);
            XImage yImage = XImage.FromFile(other_path);

            // Calculate the scaling factor to fit the image on the page while preserving the aspect ratio
            double imageRatio = (double)xImage.PixelWidth / xImage.PixelHeight;
            double pageRatio = (double)firstPic.Width / firstPic.Height;

            double width, height;

            // Determine dimensions to fit image within page
            if (imageRatio > pageRatio)
            {
                // Image is wider than page aspect ratio; fit width
                width = firstPic.Width;
                height = firstPic.Width / imageRatio;
            }
            else
            {
                // Image is taller than page aspect ratio; fit height
                height = firstPic.Height;
                width = firstPic.Height * imageRatio;
            }

            // Calculate position to center the image on the page
            double xPosition = (firstPic.Width - width) / 2;
            double yPosition = (firstPic.Height - height) / 2;

            XGraphics graphicsElement = XGraphics.FromPdfPage(firstPic);
            XGraphics graphicElement = XGraphics.FromPdfPage(secondPic);
            XRect rect = new XRect(xPosition, yPosition, width, height);
            graphicsElement.DrawImage(xImage, rect);
            graphicElement.DrawImage(yImage, rect);

            return document;
        }
        public static bool MassConvertToPdf(string directory)
        {
            List<PdfDocument> lst_files = new List<PdfDocument>();

            if (Directory.Exists(directory))
            {
                string[] allPics = Directory.GetFiles(directory);

                foreach (string pic in allPics)
                {
#pragma warning disable CS0612 // Typ oder Element ist veraltet
                    lst_files.Add(ConvertPictureToPdf(pic));
#pragma warning restore CS0612 // Typ oder Element ist veraltet
                }
                NameAllFiles(lst_files);

                foreach (PdfDocument doc in lst_files)
                {
                    doc.Save($"__Output__\\{doc.Info.Title}.pdf");
                    Console.WriteLine(doc.Info.Title);
                }
                return true;
            }
            else return false;
        }






        /// <summary>
        /// Iterates through the selected directory and combines two documents following each other according to name
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static List<PdfDocument> CombineAllIntoBundles(string directory)
        {
            List<PdfDocument> lst_AllFiles = new List<PdfDocument>();
            List<PdfDocument> lst_Questions = new List<PdfDocument>();
            List<PdfDocument> lst_Answers = new List<PdfDocument>();
            List<PdfDocument> lst_joined = new List<PdfDocument>();

            string[] arr = Directory.GetFiles(directory, "*.pdf", SearchOption.TopDirectoryOnly);

            //Array.Sort(arr, (x, y) => string.Compare(Path.GetFileName(x), Path.GetFileName(y)));


            int i = 0;
            for (int j = 0; j < arr.Length; j++)
            {

                PdfDocument pdf = PdfReader.Open(arr[i], PdfDocumentOpenMode.Import);
                pdf.Info.Title = Path.GetFileName(arr[i]);
                lst_AllFiles.Add(pdf);

                foreach (PdfDocument doc in lst_AllFiles)
                {
                    if (int.Parse(pdf.Comment) == int.Parse(doc.Comment))
                    {
                        lst_Questions.Add(pdf);
                        Console.WriteLine($"q {i} ---> {pdf.Info.Title}");
                        PdfDocument temp = CombineTwoPDFs(pdf, doc);
                        lst_joined.Add(temp);
                        i++;
                    }
                }
            }

            string currDirectory = Directory.GetCurrentDirectory() + "\\";
            var newList = new List<PdfDocument>();

            for (int y = 0; y < lst_joined.Count; y++)
            {
                string path = $"Output\\{y}.pdf";
                lst_joined[y].Save(Path.Combine(currDirectory, path));
                newList.Add(lst_joined[y]);
            }
            Console.WriteLine("Ahuhu");
            lst_joined.Clear();

            return newList;
        }


        /// <summary>
        /// Combines all PDF files in a directory into a single document.
        /// </summary>
        public static bool CombineAllInDirectory(string directory)
        {
            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException($"Directory not found: {directory}");

            string[] pdfFiles = Directory.GetFiles(directory, "*.pdf", SearchOption.TopDirectoryOnly);
            if (!pdfFiles.Any())
                return false;

            PdfDocument targetDoc = new PdfDocument();
            foreach (string pdfFile in pdfFiles)
            {
                PdfDocument sourceDoc = PdfReader.Open(pdfFile, PdfDocumentOpenMode.Import);
                CopyPagesToTarget(sourceDoc, targetDoc);
            }

            string outputDirectory = "Output";
            if (!Directory.Exists(outputDirectory))
                Directory.CreateDirectory(outputDirectory);

            string outputFilePath = Path.Combine(outputDirectory, $"{targetDoc.PageCount}.pdf");
            targetDoc.Save(outputFilePath);
            return true;
        }

        /// <summary>
        /// Combines a list of PDF files into a single document.
        /// </summary>
        public static PdfDocument CombineThesePDFs(List<string> list_of_documents)
        {
            PdfDocument targetDoc = new PdfDocument();

            foreach (string docPath in list_of_documents)
            {
                PdfDocument sourceDoc = PdfReader.Open(docPath, PdfDocumentOpenMode.Import);
                CopyPagesToTarget(sourceDoc, targetDoc);
            }

            string outputDirectory = "Output";
            if (!Directory.Exists(outputDirectory))
                Directory.CreateDirectory(outputDirectory);

            string outputFilePath = Path.Combine(outputDirectory, $"{targetDoc.PageCount}.pdf");
            targetDoc.Save(outputFilePath);

            return targetDoc;
        }

        //==============================================================================

        // Combines ALL Pdf files in the folder into one & names it after its pagecount
        //public static bool CombineAllInDirectory(string directory)
        //{
        //      int count = 0;

        //      // Collects all the information
        //      PdfDocument targetDoc = new PdfDocument();
        //      targetDoc.Tag = Path.GetDirectoryName(directory);
        //      targetDoc.PageLayout = PdfPageLayout.OneColumn;


        //      //XFont font = new XFont("Verdana", 10, XFontStyle.Bold);
        //      XStringFormat format = new XStringFormat();

        //      format.Alignment = XStringAlignment.Center;
        //      format.LineAlignment = XLineAlignment.Far;

        //      try
        //      {
        //            // Reads all PDF from this folder into an array
        //            string[] arr_allFiles = Directory.GetFiles(directory, "*.pdf", SearchOption.TopDirectoryOnly);

        //            // iterates through every page of every file, copys it into the target file & counts the pages
        //            foreach (string file in arr_allFiles)
        //            {
        //                  PdfDocument source = PdfReader.Open(file, PdfDocumentOpenMode.Import);
        //                  count += source.Pages.Count;

        //                  for (int x = 0; x < count; x++)
        //                  {
        //                        targetDoc.AddPage(source.Pages[x]);
        //                  }
        //            }
        //            // Save the files with their page count for better overview
        //            targetDoc.Save($"Output\\{count}.pdf");
        //            return true;
        //      }
        //      catch
        //      {
        //            Console.WriteLine("Something went wrong with your input");
        //            return false;
        //      }
        //}

        //private string PDFCombine()
        //{
        //      string[] AllePDFDateien;

        //      PdfPage page;
        //      PdfDocument inputDocument = new PdfDocument();
        //      PdfDocument outputDocument = new PdfDocument();


        //      // Einstellungen für die Ausgabedatei
        //      // Seiten ausrichtung (One Column = Untereinander)
        //      outputDocument.PageLayout = PdfPageLayout.OneColumn;


        //      //XFont font = new XFont("Verdana", 10, XFontStyle.Bold);
        //      XStringFormat format = new XStringFormat();


        //      /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //      format.Alignment = XStringAlignment.Center;
        //      format.LineAlignment = XLineAlignment.Far;
        //      // Alle PDF Datei aus dem Ordner in dem das Programm liegt!
        //      AllePDFDateien = Directory.GetFiles("\\InputFilesHere", "*.pdf", SearchOption.AllDirectories);



        //      foreach (string datei in AllePDFDateien)
        //      {
        //            if (datei.Contains("_Zusammenfassung_Tagesabschluesse"))
        //                  continue;

        //            inputDocument = PdfReader.Open(datei, PdfDocumentOpenMode.Import);
        //            int count = inputDocument.PageCount;

        //            for (int idx = 0; idx < count; idx++)
        //            {
        //                  page = inputDocument.PageCount > idx ? inputDocument.Pages[idx] : new PdfPage();
        //                  page = outputDocument.AddPage(page);
        //            }
        //      }

        //      string filename = $"\\Output\\{DateTime.Now.AddDays(-1).ToShortDateString()}.pdf";
        //      outputDocument.Save(filename);

        //      return filename;
        //}

    }
}








//Copyright(c) 2005 - 2014 empira Software GmbH, Troisdorf (Germany)

//Permission is hereby granted, free of charge, to any person
//obtaining a copy of this software and associated documentation
//files (the "Software"), to deal in the Software without
//restriction, including without limitation the rights to use,
//copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the
//Software is furnished to do so, subject to the following
//conditions:

//The above copyright notice and this permission notice shall be
//included in all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
//EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
//OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
//NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
//HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
//WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
//FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//OTHER DEALINGS IN THE SOFTWARE.