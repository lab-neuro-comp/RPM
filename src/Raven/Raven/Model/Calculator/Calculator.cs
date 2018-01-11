using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Model.Calculator
{
    public class Calculator
    {
        /// <summary>
        /// Gets the image paths from each line of the configuration text.
        /// </summary>
        /// <param name="raw">The lines from the text parameter file.</param>
        /// <returns>The name of each image of the test.</returns>
        public static string[] GetImages(string[] raw)
        {
            return raw.Select(it => it.Split(' ')[0]).ToArray();
        }

        /// <summary>
        /// Gets the number of options for each question in the test.
        /// </summary>
        /// <param name="raw">The lines from the text parameter file.</param>
        /// <returns>The number of options for each question of the test.</returns>
        public static int[] GetNoOptions(string[] raw)
        {
            return raw.Select(it => int.Parse(it.Split(' ')[1])).ToArray();
        }

        /// <summary>
        /// Gets the number of options for each question in the test.
        /// </summary>
        /// <param name="raw">The lines from the text parameter file.</param>
        /// <returns>The number of options for each question of the test.</returns>
        public static int[] GetCorrectOptions(string[] raw)
        {
            return raw.Select(it => int.Parse(it.Split(' ')[2])).ToArray();
        }

        /// <summary>
        /// Gets the number of options for each question in the test.
        /// </summary>
        /// <param name="raw">The lines from the text parameter file.</param>
        /// <returns>The number of options for each question of the test.</returns>
        public static string[] GetSeries(string[] raw)
        {
            return raw.Select(it => it.Split(' ')[3]).ToArray();
        }

        /// <summary>
        /// Calculates final percentile of a subject based on how they performed on a test
        /// and the percentile table.
        /// </summary>
        /// <param name="percentile">The percentile table.</param>
        /// <param name="score">The number of correct answers.</param>
        /// <param name="age">The age of the subject.</param>
        /// <returns>The percentile result of the subject according to their 
        /// performance. A percentile equals 1 might indicate an error in the 
        /// test execution.</returns>
        public static int CalculateResult(string[][] percentile, int score, int age)
        {
            int column = -1;
            int result = 1;
            var intervals = percentile[0];
            int lower;
            int upper;

            // Getting age column from percentile table
            for (int index = 1; index < intervals.Length; index++)
            {
                lower = int.Parse(intervals[index].Split(' ')[0]);
                upper = int.Parse(intervals[index].Split(' ')[1]);
                if ((age >= lower) && (age <= upper))
                {
                    column = index;
                }
            }

            // Relating percentile table to correct answers
            if (column >= 0)
            {
                for (int row = 1; row < percentile.Length-1; row++)
                {
                    lower = int.Parse(percentile[row][column]);
                    upper = int.Parse(percentile[row][column+1]);
                    if ((score > lower) && (score <= upper))
                    {
                        result = int.Parse(percentile[row][0]);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the expected result for each series.
        /// </summary>
        /// <param name="table">The raw validation table.</param>
        /// <param name="score">The number of correct answers.</param>
        /// <returns>A dictionary relating each series code to the expected result.</returns>
        public static Dictionary<string, int> GetExpectedForEachSeries(string[][] table, int score)
        {
            Dictionary<string, int> outlet = new Dictionary<string, int>();
            var series = table[0].Where(it => it.Length > 0);
            var possibleLines = table.Where(it => it[0].Length > 0).Where(it => int.Parse(it[0]) == score);

            if (possibleLines.Count() != 0)
            {
                var B = possibleLines.ElementAt(0).Skip(1);
                for (int i = 0; i != series.Count(); ++i)
                {
                    outlet[series.ElementAt(i)] = int.Parse(B.ElementAt(i));
                }
            }
            
            // IDEA Return a function instead of a dictionary.
            return outlet;
        }

        /// <summary>
        /// Relates the given answers with their series.
        /// </summary>
        /// <param name="series">The list of series for each question.</param>
        /// <param name="answers">The answers given by the subject in the tests order.</param>
        /// <returns>A dictionary relating how many correct answers were given for each
        /// series.</returns>
        public static Dictionary<string, int> RelateSeriesAndAnswers(string[] series, bool[] answers)
        {
            Dictionary<string, int> outlet = new Dictionary<string, int>();
            int limit = series.Length;
            
            for (int i = 0; i < limit; ++i)
            {
                var isCorrect = (answers[i]) ? 1 : 0;
                outlet[series[i]] = (outlet.ContainsKey(series[i])) ? outlet[series[i]] + isCorrect : isCorrect;
            }

            // IDEA Return a function instead of a dictionary.
            return outlet;
        }

        /// <summary>
        /// Gets the minimum age of the test.
        /// </summary>
        /// <param name="table">The percentile table.</param>
        /// <returns>The minimum age for a test.</returns>
        public static int GetMinimumAge(string[][] table)
        {
            return table[0][1].Split(' ').Select(int.Parse).Min();
        }

        /// <summary>
        /// Gets the maximum age for a test.
        /// </summary>
        /// <param name="table">The percentile table.</param>
        /// <returns>The maximum age for a test.</returns>
        public static int GetMaximumAge(string[][] table)
        {
            return table[0].Where(it => it.Length > 0)
                           .Select(it => it.Split(' ')
                                           .Select(int.Parse)
                                           .Max())
                           .Max();
        }
    }
}
