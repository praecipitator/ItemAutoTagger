using ItemTagger;
using ItemTagger.TaggingConfigs;

namespace ItemTaggerTest
{
    [TestClass]
    public class TestTaggingProcessor
    {

        private static DefaultTaggingConfigurations defaultTaggingConfigs = new();

        [TestMethod]
        public void TestMethod1()
        {
            var taggingConf = defaultTaggingConfigs.getConfigByType(TaggingConfigType.LWIS);
            var testName = "10mm Reflex Sight (Circle)";

            Assert.AreEqual(testName, TaggingProcessor.GetCleanedName(testName, taggingConf, true, true));
            Assert.AreEqual("{Mod} 10mm Reflex Sight (Circle){{{Screw,Aluminum,Glass,Nuclear Material}}}", TaggingProcessor.GetTaggedName("{Mod}", testName, "{{{Screw,Aluminum,Glass,Nuclear Material}}}"));

            var testName2 = "{Mod} 10mm Reflex Sight (Circle)";
            Assert.AreEqual(testName, TaggingProcessor.GetCleanedName(testName2, taggingConf, true, true));
            Assert.AreEqual(testName, TaggingProcessor.GetCleanedName("{Mod} 10mm Reflex Sight (Circle){{{Screw,Aluminum,Glass,Nuclear Material}}}", taggingConf, true, true));

            Assert.AreEqual(testName2, TaggingProcessor.GetCleanedName(testName2, taggingConf, true, false));

            Assert.AreEqual("foo", TaggingProcessor.GetCleanedName("[foo]", taggingConf, true, false));
            Assert.AreEqual("foo", TaggingProcessor.GetCleanedName("(foo)", taggingConf, true, false));
            Assert.AreEqual("foo", TaggingProcessor.GetCleanedName("{foo}", taggingConf, true, false));
            Assert.AreEqual("[f]o[o]", TaggingProcessor.GetCleanedName("[f]o[o]", taggingConf, true, false));

            Assert.AreEqual("foo", TaggingProcessor.GetCleanedName("- foo", taggingConf, true, false));
            Assert.AreEqual("- foo -", TaggingProcessor.GetCleanedName("- foo -", taggingConf, true, false));

            // no clue how to create a mock mod state, so, not doing it for now
            /*
            var settings = new TaggerSettings();
            var state = 

            var test = new TaggingProcessor(
                taggingConf,
                settings,

                );
            */
        }
    }
}