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
        /// <param name="rawLines">The lines from the text parameter file.</param>
        /// <returns>The name of each image of the test.</returns>
        public static string[] GetImages(string[] raw)
        {
            return raw.Select(it => it.Split(' ')[0]).ToArray();
        }
    }
}
