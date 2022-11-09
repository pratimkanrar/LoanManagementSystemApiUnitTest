using System.Text;
using System.Text.Json;
//using Newtonsoft.Json;

namespace UnitTestProject
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void EmiCalculatorTest()
        {
            var client = new HttpClient();
            var webRequest = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5248/api/Emi/calculateemi?loanamount=1000&interestrate=10&tenure=10");
            double expected = 104.64038098938897;
            var response = client.Send(webRequest);
            using var reader = new StreamReader(response.Content.ReadAsStream());
            string actual = reader.ReadToEnd();
            Assert.AreEqual(expected.ToString(), actual);
        }
        [Test]
        public void AdminAuthTest1()
        {
            var client = new HttpClient();
            var webRequest = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5248/AdminHome/home");
            var expected = "Ok";
            var response = client.Send(webRequest);

            Assert.AreEqual(expected, response.StatusCode.ToString());
        }
        [Test]
        public void AdminAuthTest2()
        {
            var client = new HttpClient();
            var webRequest = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5248/AdminLogin/login")
            {
                Content = new StringContent("{\r\n  \"username\": \"Admin\",\r\n  \"password\": \"Passw0rd\"\r\n}", Encoding.UTF8, "application/json")
            };
            var response = client.Send(webRequest);
            using var reader = new StreamReader(response.Content.ReadAsStream());
            string token = reader.ReadToEnd();
            var client2 = new HttpClient();
            var webRequest2 = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5248/AdminHome/home");
            client2.DefaultRequestHeaders.Add("Authorization", "Bearer "+token);
            var expected = "OK";
            var response2 = client2.Send(webRequest2);

            Assert.AreEqual(expected, response2.StatusCode.ToString());
        }
    }
}