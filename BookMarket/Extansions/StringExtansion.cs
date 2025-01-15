using System.Text;

namespace BookMarket.Extansions
{
    public enum CapitalType
    {
        None = 0,
        First,
        All
    }

    public static class StringExtansion
    {
        public static string LoremIpsum(this String source, int minWords, int maxWords, int minSentences, int maxSentences, int numParagraphs, CapitalType capital = CapitalType.None)
        {
            var words = new[]{"lorem", "ipsum", "dolor", "sit", "amet", "consectetuer",
        "adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod",
        "tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat"};
            var rand = new Random();
            int numSentences = rand.Next(maxSentences - minSentences)
                + minSentences + 1;
            int numWords = rand.Next(maxWords - minWords) + minWords + 1;
            StringBuilder result = new StringBuilder();
            var first = true;
            for (int p = 0; p < numParagraphs; p++)
            {
                //result.Append("<p>");
                for (int s = 0; s < numSentences; s++)
                {
                    for (int w = 0; w < numWords; w++)
                    {
                        if (w > 0) { result.Append(" "); }
                        if (capital == CapitalType.All)
                        {
                            result.Append(words[rand.Next(words.Length)].ToUpperFirstLetter());
                        }
                        else if (capital == CapitalType.First && first)
                        {
                            first = false;
                            result.Append(words[rand.Next(words.Length)].ToUpperFirstLetter());
                        }
                        else
                        {
                            result.Append(words[rand.Next(words.Length)]);
                        }
                    }
                    result.Append(". ");
                }
                //result.Append("</p>");
            }

            return result.ToString();
        }


        public static string ToUpperFirstLetter(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;
            // convert to char array of the string
            char[] letters = source.ToCharArray();
            // upper case the first char
            letters[0] = char.ToUpper(letters[0]);
            // return the array made of the new char array
            return new string(letters);
        }
    }
}
