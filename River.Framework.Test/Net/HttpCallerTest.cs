using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using River.Framework.Net;

namespace River.Framework.Test.Net {
    [TestClass]
    public class HttpCallerTest {
        private static HttpListener s_listener;

        public HttpCallerTest() {
            s_listener = new HttpListener();
        }

        [ClassInitialize]
        public static void Initialize(TestContext context) {
            s_listener.Prefixes.Add("http://127.0.0.1:12345/");
            s_listener.Start();

            Task.Run(() => {
                while (s_listener.IsListening) {
                    var httpContext = s_listener.GetContext();

                    var httpRequest = httpContext.Request;
                    var httpResponse = httpContext.Response;

                    if (string.Equals(httpRequest.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase)) {
                        var sb = new StringBuilder();
                        sb.AppendLine("method:get");

                        var parameters = new List<string>();

                        for (int i = 0; i < httpRequest.QueryString.Count; i++) {
                            parameters.Add(httpRequest.QueryString.GetKey(i) + "=" + httpRequest.QueryString.Get(i));
                        }

                        var qstr = string.Join("&", parameters);

                        sb.AppendLine("querystring:" + qstr);


                        var body = sb.ToString();

                        var buf = Encoding.UTF8.GetBytes(body);
                        httpResponse.ContentLength64 = buf.Length;
                        httpResponse.OutputStream.Write(buf, 0, buf.Length);
                        httpResponse.OutputStream.Flush();
                    }

                    httpResponse.Close();
                }
            });


        }

        [ClassCleanup]
        public static void Cleanup() {
            s_listener.Stop();
        }

        [TestMethod]
        public void Get_WithoutParameters() {
            var url = "http://localhost:12345";

            var caller = new HttpCaller();

            var response = caller.Get(url, null);

            Console.WriteLine(response);

            StringAssert.Contains(response, "method:get");
        }

        [TestMethod]
        public void Get_WithParameters() {
            var url = "http://localhost:12345";

            var caller = new HttpCaller();
            var parameters = new HttpParameterCollection();
            parameters
                .Add("kw", "中国")
                .Add("ie", "utf-8")
                .Add("fr", "wwwt");

            var response = caller.Get(url, parameters,
                paramsEncode: Encoding.GetEncoding("gb2312"),
                updateRequest: req => {
                    req.ContentType = "text/html;charset=utf-8";
                    req.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.8");
                });

            Console.WriteLine(response);

            StringAssert.Contains(response, "kw=中国");
        }
    }
}
