using LisaKatherine.Models.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using LisaKatherine.Models;

namespace LisaKatherine.Tests
{
    
    
    /// <summary>
    ///This is a test class for FacebookServiceTest and is intended
    ///to contain all FacebookServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FacebookServiceTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for AddFacebookComment
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod()]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("C:\\Users\\Red 2\\LKB\\LisaKatherine", "/")]
        [UrlToTest("http://localhost:58451/")]
        public void AddFacebookCommentTest()
        {
            FacebookService target = new FacebookService(); 
            FBUser fbUser = new FBUser(12345, "a token", "theUsername", "f");
            string comment = "this is a facebook comment"; 
            PublishedArticles article = new PublishedArticles();
            article.articleId = 1;
            target.AddFacebookComment(fbUser, comment, article);
        }
    }
}
