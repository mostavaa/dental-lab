using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace teethLab
{
    public class myFile
    {
        public static string projectFolder = AppDomain.CurrentDomain.BaseDirectory;
        public static void AppendStringToFile(string txt , string FilePath)
        {
            if (!File.Exists(projectFolder + FilePath))
            {
                File.Create(projectFolder+FilePath);
            }
            FileStream fs = new FileStream(projectFolder+FilePath, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(txt);
            sw.Close();
            fs.Close();
        }

        public static void AppendLineToFile(string txt, string FilePath)
        {
            if (!File.Exists(projectFolder + FilePath))
            {
                File.Create(projectFolder+FilePath);
            }
            FileStream fs = new FileStream(projectFolder+FilePath, FileMode.Append );
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(txt);
            sw.Close();
            fs.Close();
        }
        
        public static string[] readAllLines(string FilePath)
        {
            if (!File.Exists(projectFolder + FilePath))
            {
                File.Create(projectFolder+FilePath);
            }
            return File.ReadAllLines(projectFolder+FilePath);
        }

        public static void DeleteStringFromFile(string FilePath , string strToBeDeleted){
            if (!File.Exists(projectFolder + FilePath))
            {
                File.Create(projectFolder+FilePath);
                return;
            }
            string [] oldLines = myFile.readAllLines(projectFolder+FilePath);
            List<string> newLines = new List<string>();
            bool found = false;
            foreach (var item in oldLines)
            {
                string newItem = item;
                if (item.Contains(strToBeDeleted))
                {
                     newItem = item.Replace(strToBeDeleted, "");
                     found = true;
                }
                newLines.Add(newItem);
            }
            if (found) 
            File.WriteAllLines(projectFolder + FilePath, newLines);
        }

        public static void DeleteLineFromFile(string FilePath, string LineToBeDeleted)
        {
            if (!File.Exists(projectFolder + FilePath))
            {
                File.Create(projectFolder+FilePath);
                return;
            }
            
            string[] oldLines = myFile.readAllLines(projectFolder + FilePath);
            List<string> newLines = new List<string>();
            bool found = false;
            foreach (var item in oldLines)
            {
                
                if (item.ToLower() == (LineToBeDeleted.ToLower()))
                {
                    found = true;
                    continue;
                }
                newLines.Add(item);
            }
            if (found)
                File.WriteAllLines(projectFolder + FilePath, newLines);
        }

    }
}