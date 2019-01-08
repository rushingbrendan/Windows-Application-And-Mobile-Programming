/*
*  FILE          : Program.cs
*  PROJECT       : PROG2120 - Windows and Mobile Programming: Assignment #3
*  PROGRAMMER    : Brendan Rushing
*  FIRST VERSION : 2018-10-19
*  DESCRIPTION   : This assignment tests the performance of data structures for retrevial time.
*  
*  The following data structures are tested:
*   1. List
*   2. ArrayList
*   3. Dictionary
*   4. HashTable
*   
*   A text file containing 466,544 strings is read and added to each data structure.
*   
*   The string "pachymeningitis" is then searched for in each datastructure. 
*   This string was in the middle of the txt file.
*   
*   The test is performed 100,000 times and the average time is recorded.

*   references: text file: https://github.com/dwyl/english-words/blob/master/words.txt
*	
*/


//INCLUDES
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
//eo INCLUDES


namespace a3
{
    class Program
    {
        static int Main(string[] args)
        {
            int testLoops = 100000;
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            TimeSpan listTotalTime = new TimeSpan();
            TimeSpan arrayListTotalTime = new TimeSpan();
            TimeSpan hashTableTotalTime = new TimeSpan();
            TimeSpan dictionaryTotalTime = new TimeSpan();


            // 1 --- LIST
            List<string> wordsList = new List<string>();

            using (StreamReader sr = new StreamReader("words.txt"))
            {
                string line;
                // Read and display lines from the file until the end of 
                // the file is reached.
                while ((line = sr.ReadLine()) != null)

                {
                    wordsList.Add(line);
                }
            }


            // 2 --- ARRAY LIST          
            ArrayList arrayList = new ArrayList();

            using (StreamReader sr = new StreamReader("words.txt"))
            {
                string line;               
                // Read and display lines from the file until the end of 
                // the file is reached.
                while ((line = sr.ReadLine()) != null)

                {                    
                    arrayList.Add(line);                   
                }
            }
            
            // 3 --- DICTIONARY            
            Dictionary<string, int> dictionaryList = new Dictionary<string, int>();

            using (StreamReader sr = new StreamReader("words.txt"))
            {
                string line;
                int key = 0;
                // Read and display lines from the file until the end of 
                // the file is reached.
                while ((line = sr.ReadLine()) != null)

                {
                    dictionaryList.Add(line,key);
                    key++;
                }
            }

            // 4 --- HASH TABLE            
            Hashtable hashTableList = new Hashtable();

            using (StreamReader sr = new StreamReader("words.txt"))
            {
                string line;
                int key = 0;
                // Read and display lines from the file until the end of 
                // the file is reached.
                while ((line = sr.ReadLine()) != null)

                {                    
                    hashTableList.Add(key, line);
                    key++;
                }
            }

            


            for (int i = 0; i < testLoops; i++)
            {
                bool result = false;

                // LIST SEARCH
                stopwatch.Start();  //start timer
                result = wordsList.Contains("pachymeningitis");
                stopwatch.Stop();   //stop timer
                listTotalTime = listTotalTime + stopwatch.Elapsed;
                stopwatch.Reset();  //reset timer

                // ARRAY LIST SEARCH
                stopwatch.Start();  //start timer
                result = arrayList.Contains("pachymeningitis");
                stopwatch.Stop();   //stop timer
                arrayListTotalTime = arrayListTotalTime + stopwatch.Elapsed;
                stopwatch.Reset();  //reset timer

                // DICTIONARY SEARCH
                stopwatch.Start();  //start timer
                result = dictionaryList.ContainsKey("pachymeningitis");
                stopwatch.Stop();   //stop timer
                dictionaryTotalTime = dictionaryTotalTime + stopwatch.Elapsed;
                stopwatch.Reset();  //reset timer

                // HASH TABLE SEARCH
                stopwatch.Start();  //start timer
                result = hashTableList.Contains("pachymeningitis");
                stopwatch.Stop();   //stop timer                                
                hashTableTotalTime = hashTableTotalTime + stopwatch.Elapsed;
                stopwatch.Reset();  //reset timer



            }

            TimeSpan averageListTime = new TimeSpan(listTotalTime.Ticks / testLoops);
            TimeSpan averageArrayListTime = new TimeSpan(arrayListTotalTime.Ticks / testLoops);
            TimeSpan averageDictionaryTime = new TimeSpan(dictionaryTotalTime.Ticks / testLoops);
            TimeSpan averageHashTableTime = new TimeSpan(hashTableTotalTime.Ticks / testLoops);

            Console.WriteLine("Searched for string: pachymeningitis ---- 100,000 times");
            Console.WriteLine("List time: \t\t - {0}", averageListTime);
            Console.WriteLine("ArrayList time: \t - {0}", averageArrayListTime);
            Console.WriteLine("Dictionary time: \t - {0}", averageDictionaryTime);
            Console.WriteLine("HashTable time: \t - {0}", averageHashTableTime);

            return 0;

        }
    }
}
