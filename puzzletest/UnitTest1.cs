using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace puzzletest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestHorizontal()
        {
            var p = new cwregex.Puzzle();

            var strings = new string[] {"jukebox",
                                        "mezquits",
                                        "amberjack",
                                        "jargonized",
                                        "lacquerwork",
                                        "conjunctival",
                                        "swashbuckling",
                                        "CONJUNCTIVAL",
                                        "LACQUERWORK",
                                        "JARGONIZED",
                                        "AMBERJACK",
                                        "MEZQUITS",
                                        "JUKEBOX"};

            for (int i = 0; i < 13; i++)
            {
                p.Set(cwregex.Direction.Horizontal, i, strings[i]);
            }

            Console.WriteLine(p.ToString());

            for (int i = 0; i < 13; ++i)
            {
                var s = p.Get(cwregex.Direction.Horizontal, i);
                Console.WriteLine(s);
                Assert.AreEqual(strings[i], s);
            }
        }

        [TestMethod]
        public void TestVerticalA()
        {
            var p = new cwregex.Puzzle();

            var strings = new string[] {       "abcdefg",
                                              "bcdefghi",
                                             "cdefghijk",
                                            "defghijklm",
                                           "efghijklmno",
                                          "fghijklmnopq",
                                         "GHIJKLMNOPQRS",
                                          "FGHIJKLMNOPQ",
                                           "EFGHIJKLMNO",
                                            "DEFGHIJKLM",
                                             "CDEFGHIJK",
                                              "BCDEFGHI",
                                               "ABCDEFG"};

            var solutions = new string[]
            {
                "ABCDEFG",
                "BCDEFGHf",
                "CDEFGHIge",
                "DEFGHIJhfd",
                "EFGHIJKigec",
                "FGHIJKLjhfdb",
                "GHIJKLMkigeca",
                "IJKLMNljhfdb",
                "KLMNOmkigec",
                "MNOPnljhfd",
                "OPQomkige",
                "QRpnljhf",
                "Sqomkig"
            };

            for (int i = 0; i < 13; i++)
            {
                p.Set(cwregex.Direction.Horizontal, i, strings[i]);
            }

            Console.WriteLine(p.ToString());

            for (int i = 0; i < 13; ++i)
            {
                var s = p.Get(cwregex.Direction.VerticalA, i);
                Console.WriteLine(s);
                Assert.AreEqual(solutions[i], s);
            }
        }

        [TestMethod]
        public void TestVerticalB()
        {
            var p = new cwregex.Puzzle();

            var strings = new string[] {       "abcdefg",
                                              "bcdefghi",
                                             "cdefghijk",
                                            "defghijklm",
                                           "efghijklmno",
                                          "fghijklmnopq",
                                         "ghijklmnopqrs",
                                          "FGHIJKLMNOPQ",
                                           "EFGHIJKLMNO",
                                            "DEFGHIJKLM",
                                             "CDEFGHIJK",
                                              "BCDEFGHI",
                                               "ABCDEFG"};

            var solutions = new string[]
            {
                "abcdefg",
                "bcdefghF",
                "cdefghiGE",
                "defghijHFD",
                "efghijkIGEC",
                "fghijklJHFDB",
                "ghijklmKIGECA",
                "ijklmnLJHFDB",
                "klmnoMKIGEC",
                "mnopNLJHFD",
                "opqOMKIGE",
                "qrPNLJHF",
                "sQOMKIG"
            };

            for (int i = 0; i < 13; i++)
            {
                p.Set(cwregex.Direction.Horizontal, i, strings[i]);
            }

            Console.WriteLine(p.ToString());

            for (int i = 0; i < 13; ++i)
            {
                var s = p.Get(cwregex.Direction.VerticalB, i);
                Console.WriteLine(s);
                Assert.AreEqual(solutions[i], s);
            }
        }
    }
}