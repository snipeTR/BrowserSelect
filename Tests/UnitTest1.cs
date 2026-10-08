using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BrowserSelect;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestPatternGeneration()
        {
            var tests = (new[]
            {
                new[]{"http://google.com", "*.google.com"},
                new[]{"http://www.google.com", "*.google.com"},
                new[]{"http://google.au", "*.google.au"},
                new[]{"http://something.com.au", "*.something.com.au"},
                new[]{"http://www.info.com", "*.info.com"},
                new[]{"http://info.com", "*.info.com"},
                new[]{"http://something.info.au", "amg"},
                new[]{"http://linux.conf.au", "amg"},
                new[]{"http://news.vic.au", "amg"},
                new[]{"http://www.news.vic.au", "*.news.vic.au"},
                new[]{"http://www.something.info.au", "*.something.info.au"},
                new[]{"http://something.id.au", "*.something.id.au"},
                new[]{"http://localhost", "*.localhost"}
            });
            foreach (var test in tests)
            {
                var rule = Form1.generate_rule(test[0]);
                var check = "bug";
                switch (rule.mode)
                {
                    case 3:
                        check = "amg"; break;
                    case 2:
                        check = rule.second_rule; break;
                    case 1:
                        check = rule.tld_rule; break;
                }
                Assert.AreEqual(check, test[1]);
            }
        }

        private static bool Match(string type, string pattern, string url, string source = null)
        {
            var rule = new AutoMatchRule { MatchType = type, Pattern = pattern, Browser = "X" };
            return rule.Matches(new LinkContext(url, source));
        }

        [TestMethod]
        public void TestRuleMatchTypes()
        {
            Assert.IsTrue(Match("Domain", "*.google.com", "https://mail.google.com/x"));
            Assert.IsFalse(Match("Domain", "github.com", "https://gitlab.com/"));
            Assert.IsTrue(Match("URL", "github.com/snipeTR/*", "https://github.com/snipeTR/BrowserSelect"));
            Assert.IsFalse(Match("URL", "github.com/other/*", "https://github.com/snipeTR/x"));
            Assert.IsTrue(Match("Path", "/watch*", "https://www.youtube.com/watch?v=1"));
            Assert.IsFalse(Match("Path", "/docs/*", "https://x.com/blog/a"));
            Assert.IsTrue(Match("Keyword", "zoom, meet", "https://us02web.zoom.us/j/1"));
            Assert.IsTrue(Match("Extension", "pdf, .zip", "https://x.com/a/file.PDF"));
            Assert.IsTrue(Match("Extension", "html", "file:///C:/tmp/page.html"));
            Assert.IsTrue(Match("Source App", "outlook.exe", "https://x.com", @"C:\Program Files\Office\OUTLOOK.EXE"));
            Assert.IsTrue(Match("Source App", "slack", "https://x.com", @"C:\Users\a\slack.exe"));
            Assert.IsFalse(Match("Source App", "outlook", "https://x.com", null));
            Assert.IsTrue(Match("Regex", @"^https://(www\.)?example\.(com|org)/", "https://www.example.org/q"));
            Assert.IsFalse(Match("Regex", "(", "https://x"));
        }

        [TestMethod]
        public void TestRuleSerialization()
        {
            // rules saved by older versions only have pattern, browser and private flag
            AutoMatchRule old = "a.com[#!][$~][?_]Chrome";
            Assert.AreEqual("Domain", old.MatchType);
            Assert.AreEqual("", old.Arguments);
            Assert.IsFalse(old.IsPrivate);

            AutoMatchRule rt = new AutoMatchRule
            {
                Pattern = "p", Browser = "B", IsPrivate = true, MatchType = "keyword", Arguments = "--x"
            }.ToString();
            Assert.AreEqual("Keyword", rt.MatchType);
            Assert.AreEqual("--x", rt.Arguments);
            Assert.IsTrue(rt.IsPrivate);
        }
    }
}
