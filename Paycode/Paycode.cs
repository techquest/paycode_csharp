using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interswitch;
using System.IdentityModel.Tokens;



namespace Interswitch
{
    public class Paycode
    {
        Interswitch interswitch;

        public Paycode(String clientId, String clientSecret, String environment = null)
        {
            interswitch = new Interswitch(clientId, clientSecret, environment);
        }

        public Dictionary<string, string> GetEWallet(string accessToken)
        {
          return interswitch.SendWithAccessToken("/api/v1/ewallet/instruments", "GET", accessToken);
        }

        public Dictionary<string, string> GenerateWithEWallet(string accessToken, string paymentToken, string expDate, string cvv, string pin, string amt, string fep, string tranType, string pwmChannel, string tokenLifeInMin, string otp)
        {
            Random rand = new Random();
            string ttid = rand.Next(999).ToString();
            var tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken secToken = (JwtSecurityToken) tokenHandler.ReadToken(accessToken);
            var payload = secToken.Payload;
            object msisdnObj = "";
            payload.TryGetValue("mobileNo", out msisdnObj);
            string msisdn = msisdnObj.ToString();
            Dictionary<string, string> secure = interswitch.GetSecureData(null, expDate, cvv, pin, null, msisdn, ttid.ToString());
            string secureData;
            string pinData;
            string mac;
            bool hasSecureData = secure.TryGetValue("secureData", out secureData);
            bool hasPinBlock = secure.TryGetValue("pinBlock", out pinData);
            bool hasMac = secure.TryGetValue("mac", out mac);
            Dictionary<string, string> httpHeader = new Dictionary<string, string>();
            httpHeader.Add("frontendpartnerid", fep);

            var req = new
            {
                amount = amt,
                ttid = ttid,
                transactionType = tranType,
                paymentMethodIdentifier = paymentToken,
                payWithMobileChannel = pwmChannel,
                tokenLifeTimeInMinutes = tokenLifeInMin,
                oneTimePin = otp,
                pinData = pinData,
                secure = secureData,
                macData = mac
            };

            return interswitch.SendWithAccessToken("/api/v1/pwm/subscribers/" + msisdn + "/tokens", "POST", accessToken, req, httpHeader);
        }

    }
}
