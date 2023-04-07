using System;
using System.Xml;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Threading; 
using System.Windows;
using System.Windows.Forms;


public class Operator
{
    public static string[] xmlFiles;
    private TextBox textBox1;

    public static void Main()
    {
        //CREATE TEXT FILE
 
        Console.WriteLine("HI FREN!");

        this.fbdSave = new System.Windows.Forms.FolderBrowserDialog();

        Console.WriteLine("Enter the top level directory containing your defs: ");
       
        string filepath = Console.ReadLine(); 
        
        Console.WriteLine("Enter file output location: ");
        string outputLocation = Console.ReadLine(); 

        Console.WriteLine("Cheers. ");

        xmlFiles = Directory.GetFiles(filepath, "*.xml", SearchOption.AllDirectories);
        pullIt(filepath, outputLocation);

    }

    public static void pullIt(string fp, string ol){
        //CREATE TEXT FILE

        Console.WriteLine("Which tag are you interested in?");
        string tagLabel = Console.ReadLine();

        Console.WriteLine("Which properties of that tag do you want? (comma separated values, no spaces)");
        string propLong = Console.ReadLine(); 
        string[] props = propLong.Split(',');
        foreach(string p in props){
            Console.WriteLine(p);
        }


        using (StreamWriter outputFile = File.CreateText(ol + @"\" + tagLabel + ".txt"))
        {
            xmlFiles = Directory.GetFiles(fp, "*.xml", SearchOption.AllDirectories);
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
                if(file.Contains("Languages")){
                    continue;
                } try {
                    // OPEN THE FILE
                    XmlDocument xmlDoc= new XmlDocument();              // Create an XML document object ;
                    xmlDoc.Load(file);                                  // Load the XML document from the specified file ;


                    // FOR EACH HEADNODE
                    XmlNodeList defs = xmlDoc.GetElementsByTagName(tagLabel); // get list of ResearchProjectDef

                    if(defs.Count == 0){
                        //Console.WriteLine( "No ResearchProjectDef found in this file.");
                    }else{
                        for(int k=0; k < defs.Count; k++){
                            XmlNode thisNode = defs[k];
                            string oneLine = "";

                            if (thisNode.HasChildNodes)                                             // if this node has children
                            {
                                XmlNodeList properties = thisNode.ChildNodes;

                                // for each property we mentioned
                                foreach (string p in props){

                                    XmlNode property = thisNode[p];
                                    // if it has children (that are not text... WHAT?!?!)
                                    if (property.HasChildNodes && property.ChildNodes[0].NodeType != XmlNodeType.Text)
                                    {
                                    
                                        //list them separated by comma
                                        foreach (XmlNode c in property){
                                            oneLine += c.InnerText + ",";
                                        }
                                        oneLine += ";";
                                        
                                        
                                    }
                                    else{
                                        //else sparate by semicolon
                                        oneLine += property.InnerText + ";";
                                    }
                                }
                            }
                            
                            string formatString = oneLine.TrimEnd(';');
                            formatString = formatString.TrimEnd(',');
                            outputFile.WriteLine(formatString);
                        } 
                    } 
                } catch (Exception ex){
                    continue;
                }

                
            }

            outputFile.Close();
            Console.WriteLine("....................................");
        }


        Console.WriteLine("Another one? (Y/N)");
        string contin = Console.ReadLine();
        if(contin == (string)"Y" || contin == (string)"y"){
            pullIt(fp, ol);
        }else{
            Console.WriteLine("OK BYE. Press RETURN key to exit.");
            Console.ReadLine(); 
        }

    }

     public static bool isNull(string word){
        if(word != null){
             //Console.WriteLine(word + " found");
            return false;
        }else{
            //Console.WriteLine(word + " could NOT be found");
            return true;
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

