using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;


namespace regExFileCleaner
{
    class Program
    {
        static void Main(string[] args)
        {
            String fileExt;
            String outfile;
            String path = @"/*Directory you want to read the files from*/";
            String cleanPath = @"/*Directory you want to send the new files to*/";
            String cleanFilePath;
            DirectoryInfo di;
            StreamWriter sw = null;
            StreamReader sr = null;
            Regex re = null;

            //Try to create a new directory of the cleaned files 
            try
            {
                if (Directory.Exists(cleanPath)) /*Check if it already exists*/
                    Console.WriteLine("That path exists already.");
                else
                    di = Directory.CreateDirectory(cleanPath);
            }
            catch (Exception e)
            {
                Console.WriteLine(String.Format("Error \n {0}", e.Message));
            }

            foreach (var file in Directory.GetFiles(path /*, (optional extension ex. "report*.aspx" )*/))
            {
                using( sr = new StreamReader(file) )
                {
                    string myfile = sr.ReadToEnd();
                    sr.Close();

                    //Use Regular Expression to clean up any inline styles on the webpage
                    //ex. <div style="border-style: groove; position:absolute; top: 334px; left: 6px; height: 684px; width: 570px;">
                    re = new Regex("style=\"[a-z0-9:;# \\-]+\"", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    outfile = re.Replace(myfile, "");

                    //Check to make sure all of the style tags were deleted for the file
                    if(Regex.IsMatch(outfile,"style="))
                        Console.WriteLine("There are still inline styles in the file");
                
                    //Get the extension of the file to place in the clean directory of files
                    fileExt = file.Substring(file.LastIndexOf(@"\") + 1);

                    //Concatanate the clean directory path and the file extension to get the full path to write to
                    cleanFilePath = String.Format(@"{0}{1}",cleanPath,fileExt);
                
                    //Write to the clean file path
                    sw = new StreamWriter(cleanFilePath);
                    sw.Write(outfile);
                    sw.Close();    
                }
            }
            sw.Dispose();
            return;
        }
    }
}
