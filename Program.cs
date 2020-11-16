using System;
using System.Collections.Generic;


namespace Hangman_dotnet
{
	class FileParserBase {
      private int _lineNum;
	  private String _name;
	  private Dictionary<String, String> _capitalsNamesDict = new Dictionary<string, string>();
	  
	  public FileParserBase(string file_name) { //Parameterized constructor
	  String[] lines_temp = System.IO.File.ReadAllLines(@file_name);
	  
	  int i_temp = 0;
	  foreach (string line in lines_temp) {
		  i_temp++;
		  String[] line_t = line.Split(" | ", 2, StringSplitOptions.RemoveEmptyEntries);
		  _capitalsNamesDict.Add(line_t[0], line_t[1]);		  
	  }
	  _lineNum = i_temp;
	  _name = file_name;
	  }

	  public int GetNumLines() {
         return _lineNum;
      }
	  public Dictionary<String, String>  GetCapitalNamesDict() {
		  return _capitalsNamesDict;
		  }
	  public String GetRandomCountry() {
			List<string> keyList = new List<string>(_capitalsNamesDict.Keys);
			Random rand = new Random();
			string randomKey = keyList[rand.Next(keyList.Count)];
			return randomKey;
		}
	  public String GetCountryCapital(String RandomCountry) {
			return _capitalsNamesDict[RandomCountry];
		}
	}
	

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Lets start with this... \n");
			// Capitals c1 = new Capitals("countries_and_capitals.txt");
			FileParserBase fpb = new FileParserBase("countries_and_capitals.txt");
			// Get Random Country-Capital:
			String RCountry = fpb.GetRandomCountry();
			String RCapital = fpb.GetCountryCapital(RCountry);
			Console.WriteLine("RandomCountry: " + RCountry + " RandomCapital: " + RCapital);
        }
    }
}