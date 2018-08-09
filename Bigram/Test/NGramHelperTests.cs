using System;
using System.Text;
using Web.Utilities;
using Xunit;

namespace Test
{
    public class NGramHelperTests
    {
        private NGramHelper _nGramHelper = new NGramHelper();

        [Fact]
        public void TestGetNGramsBasicSuccess()
        {
            // this verifies a basic success so we know the method works and the two parameters are valid inputs
            Assert.True(_nGramHelper.GetNGrams("this is good text", 2) != null);
        }

        [Fact]
        public void TestGetNGramsInvalidText()
        {
            Assert.Throws<Exception>(() => _nGramHelper.GetNGrams(null, 2));
            
            Assert.Throws<Exception>(() => _nGramHelper.GetNGrams(string.Empty, 2));

            Assert.Throws<Exception>(() => _nGramHelper.GetNGrams(" ", 2));

            Assert.Throws<Exception>(() => _nGramHelper.GetNGrams("                  ", 2));

            Assert.Throws<Exception>(() => _nGramHelper.GetNGrams("?", 1)); // not a word

            Assert.Throws<Exception>(() => _nGramHelper.GetNGrams("two words", 3)); // less than 'n' words
        }

        [Fact]
        public void TestGetNGramsInvalidSize()
        {
            Assert.Throws<Exception>(() => _nGramHelper.GetNGrams("this is good text", 0));
            
            Assert.Throws<Exception>(() => _nGramHelper.GetNGrams("this is good text", -1));

            // n can't be larger than the number of words
            Assert.Throws<Exception>(() => _nGramHelper.GetNGrams("this is good text", 5));

            // n can be equal to the number of words
            Assert.True(_nGramHelper.GetNGrams("this is good text", 4) != null);
        }

        [Fact]
        public void TestGetNGramsEdgeCapture()
        {
            var text = "the start the middle the end";
            var dict = _nGramHelper.GetNGrams(text, 2);

            Assert.True(dict.ContainsKey("the start"));
            Assert.True(dict.ContainsKey("the middle"));
            Assert.True(dict.ContainsKey("the end"));
        }

        [Fact]
        public void TestGetNGramsValidDictionary()
        {
            var text = "The quick brown fox and the quick blue hare";
            var dict = _nGramHelper.GetNGrams(text, 2);

            Assert.True(dict.Count == 7);

            Assert.True(dict["the quick"] == 2);

            Assert.True(dict["fox and"] == 1);
        }

        [Fact]
        public void TestGetNGramsCaseSensitivity()
        {
            var text = "ALL CAPS should match with all caps";
            var dict = _nGramHelper.GetNGrams(text, 2);

            Assert.True(dict["all caps"] == 2);
        }

        [Fact]
        public void TestGetNGramsRemovePunctuation()
        {
            var text = "The quick! brown fox? and the quick blue' hare.";
            var dict = _nGramHelper.GetNGrams(text, 2);

            Assert.True(dict["the quick"] == 2);
            Assert.True(dict["quick brown"] == 1);
            Assert.True(dict["brown fox"] == 1);
            Assert.True(dict["fox and"] == 1);
            Assert.True(dict["and the"] == 1);
            Assert.True(dict["quick blue"] == 1);
            Assert.True(dict["blue hare"] == 1);
        }

        [Fact]
        public void TestGetNGramsContraction()
        {
            var text = "dont and don't do not match";
            var dict = _nGramHelper.GetNGrams(text, 1);

            Assert.True(dict["dont"] == 1);
            Assert.True(dict["don't"] == 1);
        }

        [Fact]
        public void TestGetNGramsLargeText()
        {
            var text = new string('a', 5000);
            Assert.True(_nGramHelper.GetNGrams(text, 1) != null);

            text = "word ";
            var builder = new StringBuilder();
            for (int i = 0; i < 5000; i++)
                builder.Append(text);

            text = builder.ToString();
            Assert.True(_nGramHelper.GetNGrams(text, 1) != null);
            Assert.True(_nGramHelper.GetNGrams(text, 2) != null);
            Assert.True(_nGramHelper.GetNGrams(text, 3) != null);
        }
    }
}
