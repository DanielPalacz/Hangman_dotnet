using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Diagnostics;


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
		
		private int GetNumLines() {
			return _lineNum;
		}
		
		private Dictionary<String, String>  GetCapitalNamesDict() {
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
		private String _lettersProvidedByPlayer;
		private Boolean _gameEnd;
		private Boolean _gameEndWin;
		private int _lifePoints;
		private int _guessingTries;

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
			_gameEndWin = false;
			_lifePoints = 10;
			_lettersProvidedByPlayer = "";
			_guessingTries = 0;
		}

		private String GetGuessedCountry() {
			return _country;
		}
		private Char [] GetGuessedCapitalCharsNotWhite() {
			return _capitalCharsNotWhite;
		}

		public void RunGame(Player PlayerObj) {
			Console.Clear();
			Console.WriteLine("Hello in the Hangman! Lets play!\n\n");
			PrintGameInstructionBoard();
			Stopwatch gameWatcher = Stopwatch.StartNew();
			while (! _gameEnd) {
				_guessingTries += 1;
				PrintGameState();
				Char nextMove = GetPlayerGameNextTypeOfMove();
				if (nextMove == 'L' | nextMove == 'l') {
					UpdateGameState(PlayerObj.ProvideLetter());
				}
				else if (nextMove == 'W' | nextMove == 'w') {
					UpdateGameState(PlayerObj.GuessWord());
				}
				else {
					Console.WriteLine("\nIncorrect Value or lack of decision in 5 seconds. You lost 1 Life Point.");
					UpdateGameState(-1);
				}
				PrintHint();
			}
			gameWatcher.Stop();
			if (_gameEndWin) {
				Console.WriteLine();
				Console.WriteLine("You Win. The word/capital was guessed by you in {0} game iteration. It took you {1} seconds.", 
					_guessingTries, gameWatcher.ElapsedMilliseconds / 1000);
				//Console.WriteLine("{0}|{1}/10|{2}sec|{3}|", DateTime.Now, _lifePoints, gameWatcher.ElapsedMilliseconds/1000, _capital);
			}
			else if (_gameEnd) {
				Console.WriteLine();
				Console.WriteLine("You failled. The word/capital was Not guessed by you in {0} game iteration. The Game took you {1} seconds.", 
					_guessingTries, gameWatcher.ElapsedMilliseconds / 1000);
			}
		}

		private Char GetPlayerGameNextTypeOfMove() {
			System.Threading.Thread.Sleep(500);
			Console.WriteLine("Would you like to guess 'single letter' or 'whole word'?");
			Console.WriteLine("---> type 'L(l)' if you want to guess 'single letter'");
			Console.WriteLine("---> type 'W(w)' if you want to guess 'whole word'");
			if (_lifePoints == 10) {
				System.Threading.Thread.Sleep(500);
				Console.WriteLine("\nCaution!!!\n---> if you dont choose neither 'L' nor 'W' then you lose 1 LP (1 Life Point).");
				Console.WriteLine("---> provide small letter, capital characters are not recognized by game logic");
				Console.WriteLine("---> you have 5 seconds to make decision (lack of decision during this means 1 LP deduction and lossing the given round)\n\n");
			}
			int cnt = 0;
			ConsoleKeyInfo c = new ConsoleKeyInfo();
			while (cnt < 6) {
				if (Console.KeyAvailable) {
					c = Console.ReadKey();
					break;
				}
				else {
					System.Threading.Thread.Sleep(1000);
				}
				cnt++;
			}

			return c.KeyChar;
		}
		
		private void PrintGameInstructionBoard() {
			Console.WriteLine("Game Instruction:");
			Console.WriteLine("-- the player starts a game with given number of Life Points");
			Console.WriteLine("-- every round the player is asked if he/she prefers to guess 'single letter' or 'whole word'");
			Console.WriteLine("----- the player has 5 seconds for making decision\n");
			Console.WriteLine("-- guessing 'single letter':");
			Console.WriteLine("----- the player loses 1 Life point if A-provided-letter is not in the guessd word");
			Console.WriteLine("----- the player has 5 seconds for action\n");
			Console.WriteLine("-- guessing 'whole word':");
			Console.WriteLine("----- the player loses 2 Life point if A-provided-word is not the same as guessd word");
			Console.WriteLine("----- the player has 10 seconds for action\n\n\n\n");
			Console.WriteLine("Are you ready to start? Type anything if yes.");
			Console.ReadKey();
			Console.Clear();
		}

		private void UpdateGameState(Char c) {
			if (_lettersProvidedByPlayer.Contains(c))
			{
				Console.WriteLine("\nLetter '{0}' was already used. You lose 1 LP", c);
				_lifePoints--;
				if (_lifePoints < 0) {
					Console.WriteLine("Game Over...");
					_lifePoints = 0;
					_gameEnd = true;
				}
			}
			else if (_capital.ToLower().Contains(c))
			{
				Console.WriteLine("\nLetter '{0}' is in the guessed word. Congratulations.", c);
				_lettersProvidedByPlayer += c.ToString().ToLower();
				bool tempBool = true;
				foreach (Char x in _capital.ToLower()) {
					if (! _lettersProvidedByPlayer.Contains(x))
					{
						tempBool = tempBool & false;
					}
				}
				if (tempBool) {
					Console.WriteLine("\nYou guessed the word '{0}'. Congratulations. Your points: '{1}'", _capital, _lifePoints);
					_gameEnd = tempBool;
					_gameEndWin = true;
				}
			}
			else {
				Console.WriteLine("\nLetter '{0}' is not in the guessed word. You lose 1 LP", c);
				_lifePoints--;
				_lettersProvidedByPlayer += c;
				if (_lifePoints < 0) {
					Console.WriteLine("Game Over...");
					_gameEnd = true;
				}
			}
		}

		private void UpdateGameState(String s) {
			if(s.ToLower() == _capital.ToLower()) {
				Console.WriteLine("\nHurra you guessed. The word it was really: {0}", _capital);
				Console.WriteLine("You finish the game with {0} life Points", _lifePoints);
				_gameEnd = true;
				_gameEndWin = true;
			}
			else if (_lifePoints < 2) {
				Console.WriteLine("You did not guess. Your Life Points ended. Game Over...");
				_lifePoints = 0;
				_gameEnd = true;
			}
			else {
				Console.WriteLine("Guess try was incorrect. You lose 2 points.");
				_lifePoints = _lifePoints - 2;
			}
		}

		private void UpdateGameState(int n) {
			_lifePoints = _lifePoints + n;
			if (_lifePoints < 0) {
				Console.WriteLine("Life points ended. Game Over...");
				_lifePoints = 0;
				_gameEnd = true;
			}
			else {
				Console.WriteLine("You have {0} Life Points.", _lifePoints);
			}
		}

		private void PrintGameState() {
			String _tempEncryptedWord = "";
			String dash = "-";
			foreach (Char c in _capital.ToLower()) {
				if (_lettersProvidedByPlayer.Contains(c) | " ".Contains(c)) {

					_tempEncryptedWord = _tempEncryptedWord + c;
				}
				else {
					_tempEncryptedWord = _tempEncryptedWord + dash;
				}
			}
			Console.WriteLine("\n\n\n" + "You are guessing the following word(s): " + _tempEncryptedWord);
			PrintNotGuessedLetters();
			Console.WriteLine("You have {0} Life Points.\n\n\n", _lifePoints);
		}

		private void PrintNotGuessedLetters() {
			Console.Write("Following provided letters were not in word: ");
			foreach (Char c in _lettersProvidedByPlayer) {
				if (!_capital.ToLower().Contains(c)) {
					Console.Write("'" + c + "', ");					
				}
			}
			Console.WriteLine();

		}
		private void PrintHint() {
			if (_lifePoints == 0 & (!_gameEnd)) {
				Console.Clear();
				Console.WriteLine("\n\nHint!!!");
				Console.WriteLine("The guessed word it is capital of {0}", _country);
			}
		}

	}
	
	class Player {
		
		public Char ProvideLetter () {
			Console.WriteLine("\nOk, you chose guessing single letter option. Please provide single letter in 5 seconds.");
			int cnt = 1;
			ConsoleKeyInfo c = new ConsoleKeyInfo();
			while (cnt < 6)
			{
				if (Console.KeyAvailable) {
					c = Console.ReadKey();
					break;
				}
				else {
					System.Threading.Thread.Sleep(1000);
				}
				cnt++;
			}

			return c.KeyChar;
		}
		
		public String GuessWord () {
			Console.WriteLine("\nOk, you chose guessing whole world option. Please provide the word in 10 seconds. Typing Enter after word is prohibited.");
			String s_out = "";
			ConsoleKeyInfo c = new ConsoleKeyInfo();
			Stopwatch sw = Stopwatch.StartNew();
			while (sw.Elapsed.TotalMilliseconds < 10000)
			{
				if (Console.KeyAvailable)
				{
					c = Console.ReadKey();
					s_out += c.KeyChar;
				}
			}
			sw.Stop();
			return s_out;
		}
	}
	
	public class TimerException: Exception {
		public TimerException(string message): base(message) {
		}
	}
	

    class Program
    {
        static void Main(String[] args) {			
			// Get Random Country-Capital:
			FileParserBase fpb = new FileParserBase("countries_and_capitals.txt");
			String RCountry = fpb.GetRandomCountry();
			while (true) {
				// Start playing:
				Game HangmanGameObj = new Game(fpb.GetCountryCapital(RCountry), RCountry);
				//Console.WriteLine("What is your name?");
				//String playerName = Console.ReadLine();
				Player FirstPlayerEver = new Player();
				HangmanGameObj.RunGame(FirstPlayerEver);
				//
				Console.WriteLine("\n\n");
				Console.WriteLine("Do you want to play again? Type 'y' or 'Y' if yes.");
				Char playFurther = Console.ReadKey().KeyChar;
				if (playFurther.ToString().ToLower() == "y") {
					Console.Clear();
					continue;
				}
				else {
					break;
				}
			}
        }
    }
}
