using NUnit.Framework;
using Xamarin.UITest;

namespace Flowers.Tests
{
    [TestFixture(Platform.Android)]
    public class Tests
    {

        private static IApp _app;
        private readonly Platform _platform;

        public Tests(Platform platform)
        {
            _platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            _app = FlowersApp.App;
        }
    
        [Test]
        public void AppLoadAndScrollAround_WorksAsExpected()
        {
            FlowersApp.Refresh();
            _app.Tap(x => x.Text("Ophrys apifera grows to a height of 15-50 centimetres (6-20 in). This hardy orchid develops small rosettes of leaves in autumn. They continue to grow slowly during winter. Basal leaves are ovate or oblong-lanceolate, upper leaves and bracts are ovate-lanceolate and sheathing. The plant blooms from mid-April to July producing a spike composed from one to twelve flowers. The flowers have large sepals, with a central green rib and their colour varies from white to pink, while petals are short, pubescent, yellow to greenish. The labellum is trilobed, with two pronounced humps on the hairy lateral lobes, the median lobe is hairy and similar to the abdomen of a bee. It is quite variable in the pattern of coloration, but usually brownish-red with yellow markings. The gynostegium is at right angles, with an elongated apex."));
            _app.Tap(x => x.Marked("Navigate up"));
            _app.Tap(x => x.Text("Bind Weed"));
            _app.Tap(x => x.Marked("Navigate up"));

            _app.Screenshot("Tapped on view with class: Button with text: refresh");
        }
        [Test]
        public void AppLoadAndScrollAround_WorksAsExpected_UsingQueries()
        {
            FlowersApp.Refresh();

            var firstItem = _app.Query(x => x.Class("FormsTextView"))[0];

            Assert.AreEqual("All flowers",firstItem.Text);
        }
    }

    public static class FlowersApp
    {
        public static readonly IApp _app;
        private static readonly Platform _platform;
        
        static FlowersApp()
        {
            _app = AppInitializer
                .StartApp(_platform);

        }

        public static IApp App => _app;

        public static void Refresh()
        {
            _app.Tap(x => x.Text("refresh"));

        }
    }
}
