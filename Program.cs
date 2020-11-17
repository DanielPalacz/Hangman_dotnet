﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Threading.Tasks;


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
			Console.ReadKey();
			Console.Clear();
			Console.WriteLine("Hi {0}. Hello in the Hangman! Lets play!\n\n", PlayerObj._name);
			PrintGameInstructionBoard();
			PrintEncryptedWord();
			while(! _gameEnd) {
				Char nextMove = GetPlayerGameNextTypeOfMove();
				if (nextMove == 'L' | nextMove == 'l') {
					Console.WriteLine("Guess letter. --------------------------------------");
					Console.ReadKey();
					// PlayerObj.ProvideLetter();
				}
				else if (nextMove == 'W' | nextMove == 'w') {
					// PlayerObj.GuessWord;
				}
				else {
					Console.WriteLine("Incorrect Value or lack of decision in 5 seconds. You lose 1 Life Point.");
					Console.ReadKey();
				}
			}
		}

		public Char GetPlayerGameNextTypeOfMove() {
			Console.WriteLine("Would you like to guess 'single letter' or 'whole word'?");
			Console.WriteLine("---> type 'L(l)' if you want to guess 'single letter'");
			Console.WriteLine("---> type 'W(w)' if you want to guess 'whole word'");
			Console.WriteLine("\nCaution!!!\n---> if you dont choose neither 'L' nor 'W' then you lose 1 LP (1 Life Point).");
			Console.WriteLine("---> you have 5 seconds to make decision (lack of decision during this means 1 LP deduction and lossing the given round)\n\n");
			// Char c = '$';
			int cnt = 1;
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

			// System.Timers.Timer aTimer;
			void SetTimer(Timer aTimer) {
				// Hook up the Elapsed event for the timer. 
				aTimer.Elapsed += OnTimedEvent;
				//aTimer.AutoReset = true;
				aTimer.Enabled = true;
			}
			void OnTimedEvent(Object source, ElapsedEventArgs e) {
				Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}", e.SignalTime);
				// aTimer.Stop();
				// aTimer.Dispose();
			}

			Char MakeDecisionInFiveSec() {
				ConsoleKeyInfo k = new ConsoleKeyInfo();
				for (int cnt = 5; cnt > 0; cnt--) {
					if (Console.KeyAvailable) {
						k = Console.ReadKey();
						break;
					}
					else {
						System.Threading.Thread.Sleep(1000);
					}
					if (cnt == 1) {
						throw new TimerException("Time Over.");
					}
				}
				return k.KeyChar;
			}


		}
		
		public void PrintGameInstructionBoard() {
			Console.WriteLine("Game Instruction:");
			Console.WriteLine("-- the player starts a game with given number of Life Points");
			Console.WriteLine("-- every round the player is asked if he/she prefers to guess 'single letter' or 'whole word'");
			Console.WriteLine("----- the player has 10 seconds for making decision\n");
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
			Console.WriteLine("\n\n\n" + "You are guessing the following word(s): " + _tempEncryptedWord + "\n\n\n");
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
			String RCapital = fpb.GetCountryCapital(RCountry);

			// Start playing:
			Game HangmanGameObj = new Game(RCapital, RCountry);
			Console.WriteLine("------------------------------------------------------------------");
			Console.WriteLine();
			Player FirstPlayerEver = new Player("John");
			HangmanGameObj.RunGame(FirstPlayerEver);


			//
			ConsoleKeyInfo k = new ConsoleKeyInfo();
			Console.WriteLine("Press any key in the next 5 seconds.");
			for (int cnt = 5; cnt > 0; cnt--) {
				if (Console.KeyAvailable) {
					k = Console.ReadKey();
					break;
				}
				else { 
					System.Threading.Thread.Sleep(1000);
				}
			}
        }
    }
}