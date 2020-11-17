using System;
using System.Collections.Generic;
using System.Text;


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
		
		~FileParserBase() {   //destructor
			Console.WriteLine("Object is being deleted");
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

	class Game {
		private String _capital;
		private String _country;
		private Char [] _capitalCharsNotWhite; // ToLower, NotWhite
		private Char [] _charsGuessedByPlayer;
		private Boolean _gameEnd;

		public Game (String RandomCapital, String RandomCountry) {
			_capital = RandomCapital;
			_country = RandomCountry;
			String _stemp = "";
			foreach (Char c in RandomCapital.ToLower()) {
				if (_stemp.Contains(c) | " ".Contains(c)) {
					continue;
				}
				else {
					_stemp = _stemp + c;
				}
			}
			_capitalCharsNotWhite = _stemp.ToCharArray();
			_gameEnd = false;

		}

		public String GetGuessedCountry() {
			return _country;
		}
		public Char [] GetGuessedCapitalCharsNotWhite() {
			return _capitalCharsNotWhite;
		}

		public void RunGame() {
			while(! _gameEnd) {
				//
			}
		}
		
		public void UpdateGameState() {
			//
		}

		public void AskPlayerAboutAction() {
			Console.WriteLine("Would you like to guess single letter or whole Word?");
			// String _typedText = Console.ReadLine();
		}
		
		public void PrintEncryptedWord() {
			String _tempEncryptedWord = "";
			String _tempGuessedChars = new string(_charsGuessedByPlayer);
			String dash = "-";
			foreach (Char c in _capital) {
				if (_tempGuessedChars.Contains(c) | " ".Contains(c)) {

					_tempEncryptedWord = _tempEncryptedWord + c;
				}
				else {
					_tempEncryptedWord = _tempEncryptedWord + dash;
				}
			}
			Console.WriteLine("Guessed word: " + _tempEncryptedWord);
		}
	}
	

    class Program
    {
        static void Main(String[] args) {			
			Console.WriteLine("Lets start with this... \n");
			FileParserBase fpb = new FileParserBase("countries_and_capitals.txt");

			// Get Random Country-Capital:
			String RCountry = fpb.GetRandomCountry();
			String RCapital = fpb.GetCountryCapital(RCountry);
			Console.WriteLine("RandomCountry: " + RCountry + " RandomCapital: " + RCapital);

			Game HangmanGameObj = new Game(RCapital, RCountry);
			Console.WriteLine(HangmanGameObj.GetGuessedCountry());
			Console.WriteLine(HangmanGameObj.GetGuessedCapitalCharsNotWhite());
			Console.WriteLine("---");
			HangmanGameObj.PrintEncryptedWord();
			//
        }
    }
}