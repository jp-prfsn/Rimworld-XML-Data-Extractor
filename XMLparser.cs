using System;
using System.Xml;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Threading; 


public class Operator
{
    public static string[] xmlFiles;

    public static string filePathToRead;
    public static string outputLocation;

    // delimiter
    public static string delim = ";";

    public static List<string> exclusions = new List<string>();


   
    public static void Main()
    {
        //CREATE TEXT FILE
        Console.WriteLine(" ");
        Console.WriteLine("Rimworld XML Data Extractor");
        Console.WriteLine("[@patchware] // 2023");
        Console.WriteLine("----------------------------");
        Console.WriteLine("https://github.com/jp-prfsn/Rimworld-XML-Data-Extractor");
        Console.WriteLine("----------------------------");
        

        getMode();

        

    }

    public static void getMode(){

        Console.WriteLine(" ");
        Console.WriteLine("type 'new' to run an extraction");
        Console.WriteLine("type 'settings' to change settings");
        Console.WriteLine("type 'exit' to close program");

        Console.WriteLine("----------------------------");

        string opt = Console.ReadLine();
        if(opt == "new"){
            Console.WriteLine("Folder location of defs to read:");
            Console.WriteLine("[ Submit blank to use default of ]");
            Console.WriteLine("[ C:\Program Files (x86)\Steam\steamapps\common\RimWorld ]");
            filePathToRead = getPath("C:\Program Files (x86)\Steam\steamapps\common\RimWorld");

            Console.WriteLine("Enter file output location: ");
            Console.WriteLine("[ Submit blank to use default of Desktop ]");
            outputLocation = getPath("Environment.GetFolderPath(Environment.SpecialFolder.Desktop)");

            Console.WriteLine("Finding files...");

            xmlFiles = Directory.GetFiles(filepath, "*.xml", SearchOption.AllDirectories);
            pullIt();

        }else if(opt == "settings"){
            Console.WriteLine(" ");
            Console.WriteLine("[1] change delimiter");
            Console.WriteLine("[2] add exclusions");
            Console.WriteLine("[3] remove exclusions");
            Console.WriteLine("[4] exit to menu");
            Console.WriteLine("----------------------------");
            string setter = Console.ReadLine();
            if(setter == "1"){
                // CHANGE A DELIMITER
                Console.WriteLine("Preferred delimiter:");
                delim = Console.ReadLine();

            }else if(setter == "2"){

                // ADD AN EXCLUSION TERM
                Console.WriteLine("Current exclusions:");
                foreach(string e in exclusions){
                    Console.WriteLine(e);
                }
                Console.WriteLine(" ");
                Console.WriteLine("New exclusion:");
                exclusions.Add(Console.ReadLine());

            }else if(setter == "3"){

                // ADD AN EXCLUSION TERM
                Console.WriteLine("Current exclusions:");
                foreach(string e in exclusions){
                    Console.WriteLine(e);
                }
                Console.WriteLine(" ");
                Console.WriteLine("New exclusion:");
                exclusions.Remove(Console.ReadLine());

            }
            

            getMode();

        }else if(opt == "exit"){
            System.Environment.Exit(1);
        }else{
            Console.WriteLine("No such command.");

            getMode();
        }
    }

    public static string getPath(string default){
        path = Console.ReadLine();
        if(path == ""){
            return default;
        }
        else if(Directory.Exists(path))
        {
            Console.WriteLine("Path OK.");
            return path;
        }else{
            Console.WriteLine("Path not found. Try again.");
            return getPath();
        }
    }

    public static void pullIt(){
        //CREATE TEXT FILE

        Console.WriteLine("Which tag are you interested in?");
        Console.WriteLine("[ e.g. ThingDef, ResearchProjDef ]");

        string tagLabel = Console.ReadLine();

        Console.WriteLine("Which properties of that tag do you want? (comma separated values, no spaces)");
        Console.WriteLine("[ e.g. label,baseCost,prequisites ]");

        string propLong = Console.ReadLine(); 
        string[] props = propLong.Split(',');

        using (StreamWriter outputFile = File.CreateText(outputLocation + @"\" + tagLabel + ".txt"))
        {
            xmlFiles = Directory.GetFiles(filePathToRead, "*.xml", SearchOption.AllDirectories);
            Console.WriteLine("Pulling " + tagLabel + "s");
            Console.WriteLine("XML Files found: " + xmlFiles.Length);
            Console.WriteLine("....................................");
            int count = 1;
            float perc = 0;
            foreach(string file in xmlFiles){

                perc = ((float)count/xmlFiles.Length) * 100;
                Console.Write("\r{0}%   ", perc);
                count++;
                string thisModName = getModName(file); 

                // filter out files that give issues or are not important
                foreach(string e in exclusions)
                    if(file.Contains(e)){
                        continue;
                    }
                }
                
                
                try {
                    // OPEN THE FILE
                    XmlDocument xmlDoc= new XmlDocument();
                    xmlDoc.Load(file);


                    // FOR EACH HEADNODE
                    XmlNodeList defs = xmlDoc.GetElementsByTagName(tagLabel);

                    if(defs.Count > 0){
                        for(int k=0; k < defs.Count; k++){
                            XmlNode thisNode = defs[k];
                            string oneLine = "";

                            if (thisNode.HasChildNodes) // if this node has children
                            {
                                // for each property we mentioned
                                foreach (string p in props){

                                    XmlNode property = thisNode[p];
                                    if(property != null){
                                        if(!property.HasChildNodes){
                                            oneLine +=  "0" + delim;
                                        }
                                        else if (property.ChildNodes[0].NodeType != XmlNodeType.Text)
                                        {
                                        
                                            //list them separated by comma
                                            foreach (XmlNode c in property){
                                                oneLine += c.InnerText + ",";
                                            }
                                            oneLine += delim;
                                            
                                            
                                        }
                                        else{
                                            //else sparate by semicolon
                                            oneLine += property.InnerText + delim;
                                        }
                                    }else{
                                        //Handle unfound tags
                                        oneLine +=  "0" + delim;
                                    }
                                }
                                oneLine += thisModName;
                            }
                            
                            string formatString = oneLine.TrimEnd(delim);
                            formatString = formatString.TrimEnd(',');
                            outputFile.WriteLine(formatString);
                        } 
                    } 
                } catch (Exception ex){
                    Console.WriteLine(ex);
                    continue;
                }

                
            }

            outputFile.Close();
            Console.WriteLine("....................................");
        }


        Console.WriteLine("Another one? (Y/N)");
        string contin = Console.ReadLine();
        if(contin == (string)"Y" || contin == (string)"y"){
            getMode();
        }else{
            Console.WriteLine("OK BYE. Press RETURN key to exit.");
            Console.ReadLine(); 
        }

    }


    public static string getModName(string fl){

        FileInfo f = new FileInfo(fl);
        string fullname = f.FullName;
        string filePath = fullname;
        string directoryName;
        int i = 0;

        string modName = ""; 

        while (filePath != null)
        {
            directoryName = Path.GetDirectoryName(filePath);
            filePath = directoryName;
            if (i == 1)
            {
                filePath = directoryName + @"\";
            }
            if(directoryName == @"C:\"){
                break;
            }
            string[] dirs = Directory.GetDirectories(filePath, "About", SearchOption.TopDirectoryOnly);
            if(dirs.Length > 0){
                XmlDocument doc = new XmlDocument();
                doc.Load(dirs[0] + @"\About.xml");
                XmlNodeList pkgIDs = doc.GetElementsByTagName("packageId");
                if(pkgIDs.Count > 0){
                    modName = pkgIDs[0].InnerText;
                }
                break;
            }
            i++;
        }
        if(modName != ""){
            return modName;
        }else{
            return null;
        }
        

    }
    
}

