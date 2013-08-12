namespace LisaKatherineTests
{
    using System.IO;

    using LisaKatherine.Services;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Newtonsoft.Json;

    [TestClass]
    public class TwitterTests
    {
        [TestMethod]
        public void GetTwitterResponse()
        {
            var t = new TwitterService();
            string json = t.GetTimeLine();

            var reader = new JsonTextReader(new StringReader(json));
            string s = string.Empty;
            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    s += string.Format("{0} = {1}\n", reader.TokenType, reader.Value);
                }
                else
                {
                    s += string.Format("{0}\n", reader.TokenType);
                }
            }

            Assert.IsNotNull(s);
        }
    }
}