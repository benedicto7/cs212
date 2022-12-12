using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace BabbleSample
{
    /// Babble framework
    /// Starter code for CS212 Babble assignment
    public partial class MainWindow : Window
    {
        private string input;               // input file
        private string[] words;             // input file broken into array of words
        private int wordCount = 200;        // number of words to babble
        public int uniqueWordCount = 0;     // number of unique words

        public MainWindow()
        {
            InitializeComponent();
        }

        private void loadButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.FileName = "Sample"; // Default file name
            ofd.DefaultExt = ".txt"; // Default file extension
            ofd.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show open file dialog box
            if ((bool)ofd.ShowDialog())
            {
                input = System.IO.File.ReadAllText(ofd.FileName);  // read file
                words = Regex.Split(input, @"\s+");       // split into array of words
                textBlock1.Text = "Loading file " + ofd.FileName + "\n" +
                                    "Total number of words: " + (words.Length - 1) + "\n"; // For some reason, words.Length gives an additional 1 word, so I minus by 1. I checked this using Microsoft Word. I think it's because each .txt starts with a space.
            }
        }

        private void analyzeInput(int order)
        {
            if (order > 0)
            {
                MessageBox.Show("Analyzing at order: " + order);
            }
        }

        private void babbleButton_Click(object sender, RoutedEventArgs e)
        {
            // Clears output
            textBlock1.Text = "";

            // Create another hashTable by calling createHashTable() based on the order of N 
            Dictionary<string, ArrayList> hashTable = createHashTable();

            // Call random to choose a random number
            Random random = new Random();

            // Choose starting point, which starts the count at 1
            string startingPoint = words[0];

            // Appends the starting word to the text block
            textBlock1.Text += startingPoint;

            // Starts at int i = 1 to generate the next 199 random words because we already chose a starting point
            for (int i = 1; i < Math.Min(wordCount, words.Length); i++)
            {
                // Checks the wordcount

                // If the next word is the same as the startingPoint, redo

                // Choose a random number for key
                int randomKeyNumber = random.Next(hashTable.Count);

                // Gets the string value of the key chosen by the random key number
                string firstWord = hashTable.Keys.ElementAt(randomKeyNumber);

                // Appends the key word into the text block
                textBlock1.Text += " " + firstWord;

                // Creates an array that contains all the element from the selected random key value
                ArrayList listsOfValuesInTheKey = hashTable[firstWord];

                // Choose a random number for the elements inside the key
                int randomElementNumber = random.Next(listsOfValuesInTheKey.Count);

                // Gets the string value of the element chosen by the random element number, and converts it from an object into a string
                string nextWord_FromFirstWord = Convert.ToString(listsOfValuesInTheKey[randomElementNumber]); 

                // Appends the next word that came from the key word array
                textBlock1.Text += " " + nextWord_FromFirstWord;
            }
        }

        // Creates the hash table based on the chosen order of N
        private Dictionary<string, ArrayList> createHashTable()
        {
            // Initialize the total number of unique word each .txt file has
            uniqueWordCount = 0;

            // Creates a new Dictionary
            Dictionary<string, ArrayList> hashTable = new Dictionary<string, ArrayList>();

            // Order of 1
            if (orderComboBox.SelectedIndex == 1)
            {
                for (int i = 0; i < words.Length; i++)
                {
                    string firstWord = words[i];

                    // If hashtable key does not contain word, then add word into key and make an array for that key
                    if (!hashTable.ContainsKey(firstWord))
                    {
                        hashTable.Add(firstWord, new ArrayList());

                        uniqueWordCount++;
                    }

                    // Adds the next word into the value, if the key value is not the last word in the .txt file. Else add nothing
                    if (i < (words.Length - 1))
                    {
                        hashTable[firstWord].Add(words[i + 1]);
                    }
                    else
                    {
                        hashTable[firstWord].Add("");
                    }
                }
                textBlock1.Text = "Total number of unique words: " + uniqueWordCount + "\n";
            }

            // Order of 2
            else if (orderComboBox.SelectedIndex == 2)
            {
                for (int i = 0; i < words.Length - 1; i++)
                {
                    string firstWord = words[i] + "-" + words[i + 1];

                    // If hashtable key does not contain word, then add word into key and make an array for that key
                    if (!hashTable.ContainsKey(firstWord))
                    {
                        hashTable.Add(firstWord, new ArrayList());
                        uniqueWordCount++;
                    }

                    // Adds the next word into the value, if the key value is not the last two word in the .txt file. Else add nothing
                    if (i < (words.Length - 2))
                    {
                        hashTable[firstWord].Add(words[i + 2]);
                    }
                    else
                    {
                        hashTable[firstWord].Add("");
                    }
                }
                textBlock1.Text = "Total number of unique words: " + uniqueWordCount + "\n";
            }

            // Order of 3
            else if (orderComboBox.SelectedIndex == 3)
            {
                for (int i = 0; i < words.Length - 2; i++)
                {
                    string firstWord = words[i] + "-" + words[i + 1] + "-" + words[i + 2];

                    // If hashtable key does not contain word, then add word into key and make an array for that key
                    if (!hashTable.ContainsKey(firstWord))
                    {
                        hashTable.Add(firstWord, new ArrayList());
                        uniqueWordCount++;
                    }

                    // Adds the next word into the value, if the key value is not the last three word in the .txt file. Else add nothing
                    if (i < (words.Length - 3))
                    {
                        hashTable[firstWord].Add(words[i + 3]);
                    }
                    else
                    {
                        hashTable[firstWord].Add("");
                    }
                }
                textBlock1.Text = "Total number of unique words: " + uniqueWordCount + "\n";
            }

            // Order of 4
            else if (orderComboBox.SelectedIndex == 4)
            {
                for (int i = 0; i < words.Length - 3; i++)
                {
                    string firstWord = words[i] + "-" + words[i + 1] + "-" + words[i + 2] + "-" + words[i + 3];

                    // If hashtable key does not contain word, then add word into key and make an array for that key
                    if (!hashTable.ContainsKey(firstWord))
                    {
                        hashTable.Add(firstWord, new ArrayList());
                        uniqueWordCount++;
                    }

                    // Adds the next word into the value, if the key value is not the last four word in the .txt file. Else add nothing
                    if (i < (words.Length - 4))
                    {
                        hashTable[firstWord].Add(words[i + 4]);
                    }
                    else
                    {
                        hashTable[firstWord].Add("");
                    }
                }
                textBlock1.Text = "Total number of unique words: " + uniqueWordCount + "\n";
            }

            // Order of 5
            else if (orderComboBox.SelectedIndex == 5)
            {
                for (int i = 0; i < words.Length - 4; i++)
                {
                    string firstWord = words[i] + "-" + words[i + 1] + "-" + words[i + 2] + "-" + words[i + 3] + "-" + words[i + 4];

                    // If hashtable key does not contain word, then add word into key and make an array for that key
                    if (!hashTable.ContainsKey(firstWord))
                    {
                        hashTable.Add(firstWord, new ArrayList());
                        uniqueWordCount++;
                    }

                    // Adds the next word into the value, if the key value is not the last five word in the .txt file. Else add nothing
                    if (i < (words.Length - 5))
                    {
                        hashTable[firstWord].Add(words[i + 5]);
                    }
                    else
                    {
                        hashTable[firstWord].Add("");
                    }
                }
                textBlock1.Text = "Total number of unique words: " + uniqueWordCount + "\n";
            }
            return hashTable;
        }

        private void orderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            analyzeInput(orderComboBox.SelectedIndex);

            // Outputs the total number of unique words when an order of 'N' is selected
            createHashTable();
        }
    }
}
