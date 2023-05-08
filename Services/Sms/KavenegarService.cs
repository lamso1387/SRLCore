 
using System.Threading.Tasks; 
using System.Collections.Generic; 
using System.Net.Http; 
using SRLCore.Model.Response.Kavenagar;
using SRLCore.Model.Enum;

namespace SRLCore.Services
{
    public class KavenegarService :ISrlService
    {
        public string api_key { get; set; }
        public string sender { get; set; }

        public KavenegarService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
        }


        public async Task<KavenegarListResponse<SendSmsResponse>> Send(List<string> mobiles, string message)
        {  
            var input = $"?receptor={string.Join(",", mobiles)}&sender={sender}&message={message}";

            var client = CreateClient(ClientType.kavenegar.ToString());

            var response = await client.GetAsync($"{api_key}{Model.Constants.KavenagarUrl.sms_send_json}{input}");

            var result = await response.Content.ReadAsAsync<KavenegarListResponse<SendSmsResponse>>(); 
             
            return result;
        }

    }


}
