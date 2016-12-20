using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens;
using Interswitch;

namespace SampleProject
{
    public class Example
    {
        static string clientId = "IKIA9614B82064D632E9B6418DF358A6A4AEA84D7218";
        static string clientSecret = "XCTiBtLy1G9chAnyg0z3BcaFK4cVpwDg/GTw2EmjTZ8=";
        static void Main(string[] args)
        {
         
            // Payment
            bool hasRespCode = false;
            bool hasRespMsg = false;
            string httpRespCode = "400";
            string httpRespMsg = "Failed";
            Random rand = new Random();
            
            string expDate2 = "1909";
            string cvv2 = "123";
            string pin2 = "1234";
            string amt2 = "500000";
            string tranType = "Withdrawal";
            string pwmChannel = "ATM";
            string tokenLifeInMin = "90";
            string onetimepin = "1234";
            string fep = "WEMA";


            // Paycode
            Paycode paycode = new Paycode(clientId, clientSecret);
            //var tokenHandler = new JwtSecurityTokenHandler();
            string accessToken = "eyJhbGciOiJSUzI1NiJ9.eyJsYXN0TmFtZSI6IkpBTSIsIm1lcmNoYW50X2NvZGUiOiJNWDE4NyIsInByb2R1Y3Rpb25fcGF5bWVudF9jb2RlIjoiMDQyNTk0MTMwMjQ2IiwidXNlcl9uYW1lIjoiYXBpLWphbUBpbnRlcnN3aXRjaGdyb3VwLmNvbSIsInJlcXVlc3Rvcl9pZCI6IjAwMTE3NjE0OTkyIiwibW9iaWxlTm8iOiIyMzQ4MDk4Njc0NTIzIiwicGF5YWJsZV9pZCI6IjIzMjQiLCJjbGllbnRfaWQiOiJJS0lBOTYxNEI4MjA2NEQ2MzJFOUI2NDE4REYzNThBNkE0QUVBODRENzIxOCIsImZpcnN0TmFtZSI6IkFQSSIsImVtYWlsVmVyaWZpZWQiOnRydWUsImF1ZCI6WyJjYXJkbGVzcy1zZXJ2aWNlIiwiaXN3LWNvbGxlY3Rpb25zIiwiaXN3LXBheW1lbnRnYXRld2F5IiwicGFzc3BvcnQiLCJ2YXVsdCJdLCJzY29wZSI6WyJwcm9maWxlIl0sImV4cCI6MTQ4MjI4MDkwNCwibW9iaWxlTm9WZXJpZmllZCI6dHJ1ZSwianRpIjoiYmVhNDU0YTAtMDVkOS00MWI3LWJmMDctMjQ1NDdlZTFkMzE3IiwiZW1haWwiOiJhcGktamFtQGludGVyc3dpdGNoZ3JvdXAuY29tIiwicGFzc3BvcnRJZCI6IjYxMWRmNzZhLWI0MzItNDczNy05YzY0LTc2MDdkYWRjYWNhZCIsInBheW1lbnRfY29kZSI6IjA1MTQxOTgxNTQ2ODUifQ.VHkD5H2i1Yjq8Oan1DmbokrQXGhfrYG_EWpkh3fUjhCKW_6YsM8z4Z_2XlVeUNpSSQjd8T7oARX_J06Gx4Vc0NT6Quc7eAY776VODiTfdZ98IADX6S8Go9VpARfZf-on_LbtVZXyfje3-HO1P9C9LyhPi1KexdBfYuE1GXKLIBdebXvvX0hLU81D_NE5yoDG8XDQqfiW4OPDyaCc8Mg7a6qk6HoCcxZSEOy6Flv2TAZdbNRpUMUBqYxOcZ8I6hdjN06AfBR3tLIja9oQA7IlWGkWxEp60R6pynFBouhY8XksX2vHU0EmoIv-3qVosS-ypEwKwEGAr7BwpFqS_RbahQ";
            var getPaymentMethodResp = paycode.GetEWallet(accessToken);
            hasRespCode = getPaymentMethodResp.TryGetValue("CODE", out httpRespCode);
            hasRespMsg = getPaymentMethodResp.TryGetValue("RESPONSE", out httpRespMsg);
            Console.WriteLine("Get Payment Methods HTTP Code: " + httpRespCode);
            Console.WriteLine("Get Payment Methods HTTP Data: " + httpRespMsg);
            if (hasRespCode && hasRespMsg && (httpRespCode == "200" || httpRespCode == "201" || httpRespCode == "202"))
            {
                Response response = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Response>(httpRespMsg);
                if (response.paymentMethods != null && response.paymentMethods.Length > 0)
                {
                    string token = response.paymentMethods[1].token;
                    var paycodeResp = paycode.GenerateWithEWallet(accessToken, token, expDate2, cvv2, pin2, amt2, fep, tranType, pwmChannel, tokenLifeInMin, onetimepin);
                    hasRespCode = paycodeResp.TryGetValue("CODE", out httpRespCode);
                    hasRespMsg = paycodeResp.TryGetValue("RESPONSE", out httpRespMsg);
                    Console.WriteLine("Generate Paycode HTTP Code: " + httpRespCode);
                    Console.WriteLine("Generate Paycode HTTP Data: " + httpRespMsg);
                    //Response response = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Response>(httpRespMsg);
                }
            }

            Console.ReadKey();
        }
                
    }

     public class Response
    {
        public string paymentId { get; set; }
        public string transactionRef { get; set; }
        public PaymentMethod[] paymentMethods { get; set; }
    }

     public class PaymentMethod
     {
         public string paymentMethodTypeCode { get; set; }
         public string paymentMethodCode { get; set; }
         public string cardProduct { get; set; }
         public string panLast4Digits { get; set; }
         public string token { get; set; }
     }
}
