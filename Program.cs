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

		public void RunGame(Player PlayerObj) {
			Console.WriteLine("Hi {0}. Hello in the Hangman!", PlayerObj._name);
			PrintEncryptedWord();
			while(! _gameEnd) {
				Char _nextMove = GetPlayerGameNextTypeOfMove();
				// PlayerObj.ProvideLetter();
			}
		}

		public Char GetPlayerGameNextTypeOfMove() {
			Console.WriteLine("Would you like to guess 1 letter or whole Word?");

			Console.WriteLine("Type L if you want to guess 1 Letter. It costs 1 Life point. You will have 5 seconds for making decision.");
			Console.WriteLine("Type W if you want to guess whole Word. It costs 2 Life points. You will have 10 seconds for making decision.");
			Char _c = Console.ReadKey().KeyChar;
			return _c;
		}
		
		public void PrintGameInstructionBoard() {
			Console.WriteLine("Game Instruiction:");
			Console.WriteLine("-- You are starting a game with given number of Life Points.");
			Console.WriteLine("-- In every Game iteration(round) are asked if yoy prefer to guess 1 letter or whole Word.");
			Console.WriteLine("Type L if you want to guess 1 Letter. It costs 1 Life point. You will have 5 seconds for making decision.");
			Console.WriteLine("Type W if you want to guess whole Word. It costs 2 Life points. You will have 10 seconds for making decision.");
		}




		
		public void UpdateGameState() {
			//
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
			Console.WriteLine("You are guessing the following word(s): " + _tempEncryptedWord + "\n\n");
		}
	}
	
	class Player {
		public String _name;
		private int _lifePoints;

		public Player (String name) {
			_name = name;
			_lifePoints = 5;
		}
		
		public Char ProvideLetter () {
			Char _letter = Console.ReadKey().KeyChar;
			return _letter;
		}
		
		public String GuessWord () {
			String _word =  Console.ReadLine();;
			return _word;
		}
	}
	

    class Program
    {
        static void Main(String[] args) {			
			// Get Random Country-Capital:
			FileParserBase fpb = new FileParserBase("countries_and_capitals.txt");
			String RCountry = fpb.GetRandomCountry();
			String RCapital = fpb.GetCountryCapital(RCountry);

			// Start playing:
			Game HangmanGameObj = new Game(RCapital, RCountry);
			Console.WriteLine("------------------------------------------------------------------");
			Console.WriteLine();
			Player FirstPlayerEver = new Player("John");
			HangmanGameObj.RunGame(FirstPlayerEver);
        }
    }
}