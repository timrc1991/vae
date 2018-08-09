using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Utilities
{
    /// <summary>
    /// This class is a helper utility used for parsing and collection of nGrams.
    /// </summary>
    public class NGramHelper
    {
        /// <summary>
        /// This method is used to find nGrams within given text, where nGrams represent a series of 'n' adjacent words.
        /// </summary>
        /// <param name="text">Text, which will be parsed to find nGrams.</param>
        /// <param name="n">Represents the size of the nGrams to be found.</param>
        /// <returns>Dictionary where the keys represent nGrams and the values represent the number of occurences of each nGram in the "text" parameter.</returns>
        public Dictionary<string, int> GetNGrams(string text, int n)
        {
            if (n < 1)
                throw new Exception($"Google \"n-grams\" then explain to me what a {n}-gram is.");

            if (string.IsNullOrWhiteSpace(text))
                throw new Exception("The supplied text must not be empty.");

            var nGramDictionary = new Dictionary<string, int>();
            var nGramBuilder = new StringBuilder(); // represents the current nGram being built as we iterate through the 'text'

            var wordQueue = new Queue<int>(); // maintains length of each word found
            int wordCount = 0; // total count of words in 'nGramBuilder' (so we know when it has 'n' words)
            int wordLength = 0; // represents the length of the last word we've found

            // iterating through characters in the 'text'
            for (int i = 0; i <= text.Length; i++)
            {
                // checks if the current char is a letter, number, or a special character attached to a surrounding word
                if (i != text.Length
                    && (char.IsLetterOrDigit(text[i])
                        || (i != 0
                            && i < (text.Length - 1)
                            && text[i] != ' '
                            && (char.IsPunctuation(text[i])
                                || char.IsSeparator(text[i]))
                            && (char.IsLetterOrDigit(text[i - 1])
                                && char.IsLetterOrDigit(text[i + 1])))))
                {
                    nGramBuilder.Append(text[i]); // append current character to the current word being built
                    wordLength++;
                }
                else
                {
                    if (wordLength > 0)
                    {
                        // moving on to next word in the 'text', so this stores the size of the word we've just built
                        wordQueue.Enqueue(wordLength);
                        wordLength = 0;
                        wordCount++;

                        // checks to see if we've found 'n' adjacent words
                        if (wordCount == n)
                        {
                            // make an nGram out of the current string builder, then add/increment in dictionary
                            var nGram = nGramBuilder.ToString().ToLower();
                            if (nGramDictionary.ContainsKey(nGram))
                                nGramDictionary[nGram]++; // increment the count of the existing nGram in the dictionary
                            else
                                nGramDictionary.Add(nGram, 1); // add new nGram to the dictionary

                            // remove 1st word in string builder since we've already used it for all possible nGrams
                            nGramBuilder.Remove(0, wordQueue.Dequeue() + (n == 1 ? 0 : 1)); // add 1 to account for space we appended
                            wordCount--;
                        }

                        // adding space to separate words in the nGram being built (don't need to for n == 1)
                        if (n != 1)
                            nGramBuilder.Append(' ');
                    }
                }
            }
            
            // check to see if there were fewer than 'n' words found in the text
            if (nGramDictionary.Count == 0)
            {
                var size = n == 1 ? "uni" : (n == 2 ? "bi" : (n == 3 ? "tri" : n + "-"));
                throw new Exception($"You must provide more than {wordCount} word{(wordCount == 1 ? "s" : "")} if you want {size}grams.");
            }

            return nGramDictionary;
        }
    }
}
