using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MVSPO_DZ_2
{
    internal class GermanNumbers
    {
        public static Dictionary<string, int> unit = new Dictionary<string, int>()
        {
            { "eins", 1 },
            { "zwei", 2 },
            { "drei", 3 },
            { "vier", 4 },
            { "fünf", 5},
            { "sechs", 6 },
            { "sieben", 7 },
            { "acht", 8 },
            { "neun", 9 },
            { "zehn", 10 }
        };

        public static Dictionary<string, int> eleven_nineteen = new Dictionary<string, int>()
        {
            { "elf", 11 },
            { "zwölf", 12 },
            { "dreizehn", 13 },
            { "vierzehn", 14 },
            { "fünfzehn", 15 },
            { "sechzehn", 16 },
            { "siebzehn", 17 },
            { "achtzehn", 18 },
            { "neunzehn", 19 }
        };

        public static Dictionary<string, int> tens = new Dictionary<string, int>()
        {
            { "zwanzig", 20 },
            { "dreißig", 30 },
            { "vierzig", 40 },
            { "fünfzig", 50 },
            { "sechzig", 60 },
            { "siebzig", 70 },
            { "achtzig", 80 },
            { "neunzig", 90 }
        };
    }

}
