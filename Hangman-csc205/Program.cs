using System;
using System.Collections.Generic;
using System.IO;

namespace Hangman
{
    // Main entry point for the application
    class Program
    {
        static void Main(string[] args)
        {
            // Display welcome message
            Console.WriteLine("Welcome to Hangman!");

            // Create an instance of the Hangman game
            HangmanGame game = new HangmanGame();

            // Start the game
            game.Play();
        }
    }

    // Class responsible for the Hangman game logic
    class HangmanGame
    {
        private string secretWord;          // The word to be guessed
        private char[] guessedWord;         // The current state of the guessed word
        private int attemptsRemaining;      // Number of attempts remaining
        private List<char> guessedLetters;  // List of letters that have been guessed

        // Constructor to initialize game settings
        public HangmanGame()
        {
            secretWord = GetRandomWord();  // Select a random word
            guessedWord = new char[secretWord.Length];

            // Initialize guessedWord with underscores
            for (int i = 0; i < guessedWord.Length; i++)
            {
                guessedWord[i] = '_';
            }

            attemptsRemaining = 6;  // Allow 6 incorrect guesses
            guessedLetters = new List<char>();  // Initialize the list of guessed letters
        }

        // Main game loop
        public void Play()
        {
            // Continue until all attempts are used or the word is guessed
            while (attemptsRemaining > 0 && !IsWordGuessed())
            {
                Console.Clear();  // Clear the console for better readability
                Console.WriteLine("Attempts remaining: " + attemptsRemaining);
                DisplayWord();  // Display the current state of the word
                Console.WriteLine("Guessed letters: " + string.Join(", ", guessedLetters));

                Console.Write("Enter your guess (a single letter): ");
                string input = Console.ReadLine();

                // Validate the input
                if (string.IsNullOrWhiteSpace(input) || input.Length != 1 || !char.IsLetter(input[0]))
                {
                    // If input is invalid, notify the user
                    Console.WriteLine("Invalid input. Please enter a single letter.");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();  // Wait for user to acknowledge
                    continue;  // Skip to the next iteration of the loop
                }

                // Convert input to lowercase to ensure case insensitivity
                char guess = char.ToLower(input[0]);

                // Check if the letter has already been guessed
                if (guessedLetters.Contains(guess))
                {
                    Console.WriteLine("You already guessed that letter. Try again.");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                    continue;
                }

                // Add the guessed letter to the list of guessed letters
                guessedLetters.Add(guess);
                GuessLetter(guess);  // Process the guessed letter
            }

            // Check the game outcome
            if (IsWordGuessed())
            {
                // If the word is guessed, display a success message
                Console.WriteLine("Congratulations! You've guessed the word: " + secretWord);
            }
            else
            {
                // If attempts are exhausted, reveal the word
                Console.WriteLine("Sorry, you've run out of attempts. The word was: " + secretWord);
            }
        }

        // Retrieve a random word from a file
        private string GetRandomWord()
        {
            // Determine the file path for words.txt
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "words.txt");

            // Read all words from the file
            string[] words = File.ReadAllLines(filePath);

            // Select a random word from the list
            Random random = new Random();
            return words[random.Next(words.Length)];
        }

        // Display the current guessed state of the word
        private void DisplayWord()
        {
            // Join the guessedWord array into a string with spaces
            Console.WriteLine("Current word: " + string.Join(" ", guessedWord));
        }

        // Process the player's letter guess
        private void GuessLetter(char guess)
        {
            bool correctGuess = false;  // Track if the guess was correct

            // Check each letter in the secret word
            for (int i = 0; i < secretWord.Length; i++)
            {
                if (secretWord[i] == guess)
                {
                    guessedWord[i] = guess;  // Reveal the letter in the guessed word
                    correctGuess = true;     // Mark the guess as correct
                }
            }

            // If the guess was incorrect, reduce the number of attempts
            if (!correctGuess)
            {
                attemptsRemaining--;
                Console.WriteLine("Incorrect guess!");
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
            }
        }

        // Check if the word has been completely guessed
        private bool IsWordGuessed()
        {
            // Compare the guessedWord array to the secret word
            return new string(guessedWord) == secretWord;
        }
    }
}
