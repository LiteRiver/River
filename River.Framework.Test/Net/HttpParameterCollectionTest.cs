using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using River.Framework.Net;
using System.Text;
using System.Web;

namespace River.Framework.Test.Net {
    [TestClass]
    public class HttpParameterCollectionTest {

        [TestMethod]
        public void PrintAllEncodings() {
            var encodingInfos = Encoding.GetEncodings();
            foreach (var info in encodingInfos) {
                PrintProperty("Name", info.Name);
                PrintProperty("DisplayName", info.DisplayName);
                PrintProperty("CodePage", info.CodePage);
                PrintLine();
            }
        }

        [TestMethod]
        public void ToQueryString_UTF8UrlEncode() {

            var parameters = new HttpParameterCollection();
            parameters.Add("key1", "中国");
            parameters.Add("key2", "人民");
            parameters.Add("key3", "happy");
            parameters.Add("key4", null);

            var encoded = parameters.ToQueryString().ToLower();

            Console.WriteLine(encoded);

            Assert.AreEqual<string>("key1=%e4%b8%ad%e5%9b%bd&key2=%e4%ba%ba%e6%b0%91&key3=happy&key4=", encoded);

        }

        [TestMethod]
        public void ToQueryString_GbkUrlEncode() {

            var parameters = new HttpParameterCollection();
            parameters.Add("key1", "中国");
            parameters.Add("key2", "人民");
            parameters.Add("key3", "happy");



            var encoded = parameters.ToQueryString(Encoding.GetEncoding("gbk")).ToLower();

            Assert.AreEqual<string>("key1=%d6%d0%b9%fa&key2=%c8%cb%c3%f1&key3=happy", encoded);
        }

        [TestMethod]
        public void ToQueryString_Unsorted() {
            var parameters = new HttpParameterCollection(false);
            parameters.Add("key3", "中国");
            parameters.Add("key1", "人民");
            parameters.Add("key2", "happy");

            var keys = string.Join("|", parameters.Keys);
            Assert.AreEqual("key3|key1|key2", keys);
        }


        [TestMethod]
        public void ToQueryString_Sorted() {
            var parameters = new HttpParameterCollection(true);
            parameters.Add("key3", "中国");
            parameters.Add("key1", "人民");
            parameters.Add("key2", "happy");

            var keys = string.Join("|", parameters.Keys);
            Assert.AreEqual("key1|key2|key3", keys);
        }

        [TestMethod]
        public void ToQueryString_DonotEncodeWhenEncodeIsNull() {
            var parameters = new HttpParameterCollection();
            parameters.Add("key1", "中国");
            parameters.Add("key2", "人民");
            parameters.Add("key3", "happy");

            var encoded = parameters.ToString();

            Assert.AreEqual<string>("key1=中国&key2=人民&key3=happy", encoded);
        }

        private void PrintProperty(string name, object value) {
            Console.WriteLine("{0,-40}{1}", name, value);
        }

        private void PrintLine() {
            Console.WriteLine("--------------------------------------------------------------------------------------");
        }
    }
}
