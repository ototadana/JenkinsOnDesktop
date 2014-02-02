using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using XPFriend.JenkinsOnDesktop.Properties;

namespace XPFriend.JenkinsOnDesktop.Core
{
    [TestClass]
    public class ButlerFactoryTest
    {
        private CultureInfo defaultUICulture;

        [TestInitialize]
        public void Setup()
        {
            defaultUICulture = Thread.CurrentThread.CurrentUICulture;
        }

        [TestCleanup]
        public void Cleanup()
        {
            Thread.CurrentThread.CurrentUICulture = defaultUICulture;
        }

        [TestMethod]
        public void TestCreateCalmJenkinsOnJaJP()
        {
            // when
            Butler butler = ButlerFactory.CreateCalmJenkins();

            // then
            Assert.AreEqual(ButlerFactory.CalmJenkins, butler.Name);
            Assert.AreEqual(Resources.Nickname_CalmJenkins, butler.Nickname);
            Assert.AreEqual(Resources.DisplayName_CalmJenkins, butler.DisplayName);
            Assert.AreEqual(Resources.License_CalmJenkins, butler.License);
            Assert.AreEqual(ButlerFactory.Happy, butler.TypicalAppearance);

            AssertDefaultProperties(butler);

            Assert.AreEqual(2, butler.Appearances.Count);
            {
                Appearance appearance = butler.Appearances[ButlerFactory.Normal];
                AssertDefaultPropertiesOfCalmJenkins(appearance);

                Assert.AreEqual(null, appearance.MessageText);
                Assert.AreEqual(Resources.Message_Report, appearance.BalloonTipText);
                Assert.AreEqual(Resources.BalloonTip_Title, appearance.BalloonTipTitle);
            }
            {
                Appearance appearance = butler.Appearances[ButlerFactory.Happy];
                AssertDefaultPropertiesOfCalmJenkins(appearance);

                Assert.AreEqual(Resources.Message_Report, appearance.MessageText);
                Assert.AreEqual(null, appearance.BalloonTipText);
                Assert.AreEqual(null, appearance.BalloonTipTitle);
            }
        }

        private static void AssertDefaultPropertiesOfCalmJenkins(Appearance appearance)
        {
            AssertDefaultProperties(appearance);

            Assert.AreEqual("jenkins.png", appearance.ImageFile);
            Assert.AreEqual(3, appearance.EnterAnimation.Count);
            Assert.AreEqual(2, appearance.ExitAnimation.Count);
            Assert.AreEqual(false, appearance.Topmost);
        }

        private static void AssertDefaultProperties(Appearance appearance)
        {
            Assert.AreEqual(10, appearance.BalloonTipTimeout);
            Assert.IsNotNull(appearance.MessageStyle);
            Assert.AreEqual("jenkins_icon.ico", appearance.IconFile);
            Assert.AreEqual(ToolTipIcon.None, appearance.ToolTipIcon);
            Assert.IsNull(appearance.Icon);
            Assert.IsNull(appearance.Image);
            Assert.IsNotNull(appearance.Bitmap);
        }

        private static void AssertDefaultProperties(Butler butler)
        {
            Assert.AreEqual(false, butler.HasNews);
            Assert.AreEqual(null, butler.SourceUrl);
            Assert.AreEqual(null, butler.MessageText);
            Assert.AreEqual(null, butler.BalloonTipText);
            Assert.AreEqual(null, butler.BalloonTipTitle);
            Assert.AreEqual(false, butler.HasMessage);
            Assert.AreEqual(null, butler.TypicalAppearanceImage);
        }

        [TestMethod]
        public void TestCreateEmotionalJenkins()
        {
            // when
            Butler butler = ButlerFactory.CreateEmotionalJenkins();

            // then
            Assert.AreEqual(ButlerFactory.EmotionalJenkins, butler.Name);
            Assert.AreEqual(Resources.Nickname_EmotionalJenkins, butler.Nickname);
            Assert.AreEqual(Resources.DisplayName_EmotionalJenkins, butler.DisplayName);
            Assert.AreEqual(Resources.License_EmotionalJenkins, butler.License);
            Assert.AreEqual(ButlerFactory.Sad, butler.TypicalAppearance);

            AssertEmotionalJenkinsProperties(butler);
        }

        private static void AssertEmotionalJenkinsProperties(Butler butler)
        {
            AssertDefaultProperties(butler);

            Assert.AreEqual(6, butler.Appearances.Count);
            {
                Appearance appearance = butler.Appearances[ButlerFactory.Normal];
                AssertDefaultPropertiesOfEmotionalJenkins(appearance);

                Assert.AreEqual("jenkins.png", appearance.ImageFile);
                Assert.AreEqual(3, appearance.EnterAnimation.Count);
                Assert.AreEqual(2, appearance.ExitAnimation.Count);
                Assert.AreEqual(false, appearance.Topmost);
            }
            {
                Appearance appearance = butler.Appearances[ButlerFactory.Sad];
                AssertDefaultPropertiesOfEmotionalJenkins(appearance);

                Assert.AreEqual("sad.png", appearance.ImageFile);
                Assert.AreEqual(3, appearance.EnterAnimation.Count);
                Assert.AreEqual(2, appearance.ExitAnimation.Count);
                Assert.AreEqual(false, appearance.Topmost);
            }
            {
                Appearance appearance = butler.Appearances[ButlerFactory.Angry];
                AssertDefaultPropertiesOfEmotionalJenkins(appearance);

                Assert.AreEqual("oni.png", appearance.ImageFile);
                Assert.AreEqual(3, appearance.EnterAnimation.Count);
                Assert.AreEqual(2, appearance.ExitAnimation.Count);
                Assert.AreEqual(false, appearance.Topmost);
            }
            {
                Appearance appearance = butler.Appearances[ButlerFactory.Rageful];
                AssertDefaultPropertiesOfEmotionalJenkins(appearance);

                Assert.AreEqual("onibi.png", appearance.ImageFile);
                Assert.AreEqual(3, appearance.EnterAnimation.Count);
                Assert.AreEqual(2, appearance.ExitAnimation.Count);
                Assert.AreEqual(true, appearance.Topmost);
            }
            {
                Appearance appearance = butler.Appearances[ButlerFactory.Expectant];
                AssertDefaultPropertiesOfEmotionalJenkins(appearance);

                Assert.AreEqual("ninja.png", appearance.ImageFile);
                Assert.AreEqual(3, appearance.EnterAnimation.Count);
                Assert.AreEqual(4, appearance.ExitAnimation.Count);
                Assert.AreEqual(false, appearance.Topmost);
            }
            {
                Appearance appearance = butler.Appearances[ButlerFactory.Happy];
                AssertDefaultPropertiesOfEmotionalJenkins(appearance);

                Assert.AreEqual("jenkins.png", appearance.ImageFile);
                Assert.AreEqual(3, appearance.EnterAnimation.Count);
                Assert.AreEqual(2, appearance.ExitAnimation.Count);
                Assert.AreEqual(false, appearance.Topmost);
            }
        }

        private static void AssertDefaultPropertiesOfEmotionalJenkins(Appearance appearance)
        {
            AssertDefaultProperties(appearance);

            Assert.AreEqual(Resources.Message_Report, appearance.MessageText);
            Assert.AreEqual(null, appearance.BalloonTipText);
            Assert.AreEqual(null, appearance.BalloonTipTitle);
        }

        [TestMethod]
        public void TestCreateDefaultButler()
        {
            // when
            Butler butler = ButlerFactory.CreateDefaultButler("name1", "nickname1", "displayName1");

            // then
            Assert.AreEqual("name1", butler.Name);
            Assert.AreEqual("nickname1", butler.Nickname);
            Assert.AreEqual("displayName1", butler.DisplayName);
            Assert.AreEqual(Resources.License_EmotionalJenkins, butler.License);
            Assert.AreEqual(ButlerFactory.Sad, butler.TypicalAppearance);

            AssertEmotionalJenkinsProperties(butler);
        }

        [TestMethod]
        public void TestXamlSerializeOfCalmJenkinsOnJaJP()
        {
            // setup
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("ja-JP");
            string expected = TestUtil.ReadTestResource(@"Core\ButlerFactoryTest_CalmJenkins_ja-JP.txt");

            // when
            string actual = TestUtil.ToXamlString(ButlerFactory.CreateCalmJenkins());

            // then
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestXamlSerializeOfEmotionalJenkinsOnJaJP()
        {
            // setup
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("ja-JP");
            string expected = TestUtil.ReadTestResource(@"Core\ButlerFactoryTest_EmotionalJenkins_ja-JP.txt");

            // when
            string actual = TestUtil.ToXamlString(ButlerFactory.CreateEmotionalJenkins());

            // then
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestXamlSerializeOfCalmJenkinsOnEnUS()
        {
            // setup
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            string expected = TestUtil.ReadTestResource(@"Core\ButlerFactoryTest_CalmJenkins_en-US.txt");

            // when
            string actual = TestUtil.ToXamlString(ButlerFactory.CreateCalmJenkins());

            // then
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestXamlSerializeOfEmotionalJenkinsOnEnUS()
        {
            // setup
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            string expected = TestUtil.ReadTestResource(@"Core\ButlerFactoryTest_EmotionalJenkins_en-US.txt");

            // when
            string actual = TestUtil.ToXamlString(ButlerFactory.CreateEmotionalJenkins());

            // then
            Assert.AreEqual(expected, actual);
        }
    }
}
