using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using SRLCore.Model.Enum;
using SRLCore.Model.Moneyar;
using System.Net.Http.Headers;
using System.Linq;
using SRLCore.Middleware;
using SRLCore.Model;

namespace SRLCore.Services.EWallet
{
    public abstract class MoneyarService : ISrlService
    {
        public abstract string MoneyarToken { get; }// => Startup.setting.MoneyarService.token;
        public HttpClient client;

        public MoneyarService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            client = CreateClient(ClientType.moneyar.ToString());
        }


        private async Task<MoneyarResponse<MoneyarCashoutReportResponse>> CompletedCashoutReport(DateTime start_date, DateTime end_date, string iban)
        {

            Dictionary<string, string> query = new Dictionary<string, string>();
            string format = "yyyy-MM-dd";
            query[nameof(MoneyarCompletedCashoutReportRequest.Startdate)] = start_date.ToString(format);
            query[nameof(MoneyarCompletedCashoutReportRequest.Enddate)] = end_date.ToString(format);
            query[nameof(MoneyarCompletedCashoutReportRequest.Iban)] = iban;

            var response = await client.PostAsync(Model.Constants.MoneyarUrl.CompleteCashoutReport, SRL.Json.CreateJsonBody(query));

            var result = await HandleResponse<MoneyarCashoutReportResponse>(response);

            return result;
        }

        public async Task<MoneyarResponse<MoneyarTransferResponse>> Transfer(string srcUserDigitalBagId, string dscUserDigitalBagId, long amount, string des)
        {
            MoneyarTransferRequest transferAccount = new MoneyarTransferRequest()
            {
                Amount = amount,
                Description = des
            };


            transferAccount.SrcUser = new MoneyarTransferRequest.Srcuser() { uid = srcUserDigitalBagId };
            transferAccount.DestUser = new MoneyarTransferRequest.Destuser() { uid = dscUserDigitalBagId };


            var response = await client.PostAsync(Model.Constants.MoneyarUrl.transfer, SRL.Json.CreateJsonBody(transferAccount));

            if (!response.IsSuccessStatusCode) return new MoneyarResponse<MoneyarTransferResponse> { code = (int)response.StatusCode };

            var result = await response.Content.ReadAsAsync<MoneyarResponse<MoneyarTransferResponse>>();

            return result;
        }

        public async Task<MoneyarBalanceResponse> GetBalance(string uid)
        {
            Dictionary<string, object> body = new Dictionary<string, object>();
            body["uid"] = uid;



            var response = await client.PostAsync(Model.Constants.MoneyarUrl.balance, SRL.Json.CreateJsonBody(body));

            if (!response.IsSuccessStatusCode) return new MoneyarBalanceResponse { code = (int)response.StatusCode };

            var result = await response.Content.ReadAsAsync<MoneyarBalanceResponse>();

            return result;
        }

        public async Task<MoneyarResponse<MoneyarCashoutResponse>> CashOut(MoneyarCashoutRequest request)
        {


            var response = await client.PostAsync(Model.Constants.MoneyarUrl.cashout, SRL.Json.CreateJsonBody(request));
            var result = await response.Content.ReadAsAsync<MoneyarResponse<MoneyarCashoutResponse>>();

            if (!response.IsSuccessStatusCode) return new MoneyarResponse<MoneyarCashoutResponse> { code = (int)response.StatusCode, message = result.message };


            return result;
        }
        public async Task<MoneyarResponse<MoneyarTransactionDetailsReportResponse<TransEnumType>>> TransactionDetailsReport<TransEnumType>(MoneyarTransactionDetailsReportRequest request)
        {

            var response = await client.PostAsync(Model.Constants.MoneyarUrl.TransactionDetailsReport, SRL.Json.CreateJsonBody(request));

            var result = await HandleResponse<MoneyarTransactionDetailsReportResponse<TransEnumType>>(response);

            return result;
        }
        public async Task<MoneyarResponse<MoneyarCashoutReportResponse>> ReservedCashoutReport()
        {

            var response = await client.PostAsync(Model.Constants.MoneyarUrl.ReservedCashoutReport, SRL.Json.CreateJsonBody(new Dictionary<string, object> { ["startdate"] = "2021-01-08", ["enddate"] = "2030-03-12" }));


            var result = await HandleResponse<MoneyarCashoutReportResponse>(response);

            return result;
        }
        public async Task<List<MoneyarCashoutReportResponse.TRXs>> SearchCompletedCashoutReport(MoneyarCompletedCashoutReportRequest request)
        {

            var query = await CompletedCashoutReport(request.Startdate_en, request.Enddate_en, request.Iban);
            ThrowIfError(query);
            var rez = query.data.transactions;

            if (!string.IsNullOrWhiteSpace(request.sourcephonenumber))
                rez = rez.Where(x => x.sourcephonenumber == request.sourcephonenumber).ToList();

            return rez;
        }

        public async Task<MoneyarResponse<MoneyarShebaResponse>> GetShaba(string sheba)
        {
            Dictionary<string, string> inp = new Dictionary<string, string> { ["iban"] = sheba.ToUpper() };


            var response = await client.PostAsync(Model.Constants.MoneyarUrl.getiban, SRL.Json.CreateJsonBody(inp));


            var result = await HandleResponse<MoneyarShebaResponse>(response);

            return result;
        }
        public async Task<MoneyarShebaResponse> InqueryShaba(string shaba)
        {
            var result = await GetShaba(shaba);
            if (!result.IsOk && result.code == 2)
                throw new GlobalException(SRLCore.Model.ErrorCode.BadRequest, Model.Constants.MessageText.wrong_shaba);
            else if (!result.IsOk)
                throw new GlobalException(SRLCore.Model.ErrorCode.FailedDependency, result.message);
            return result.data;
        }
        public async Task<MoneyarResponse<MoneyarDepositReportResponse>> DepositReport(MoneyarDepositReportRequest request)
        {


            var response = await client.PostAsync(Model.Constants.MoneyarUrl.DepositReport, SRL.Json.CreateJsonBody(request));


            var result = await HandleResponse<MoneyarDepositReportResponse>(response);

            return result;
        }
        public async Task<MoneyarResponse<object>> Deposit(MoneyarApplyDepositRequest request)
        {


            var response = await client.PostAsync(Model.Constants.MoneyarUrl.Deposit, SRL.Json.CreateJsonBody(request));


            var result = await HandleResponse<object>(response);

            return result;
        }

        public async Task<MoneyarResponse<CashoutTransactionReportResponse>> CashoutTransactionReport(string[] uid)
        {
            Dictionary<string, string[]> inp = new Dictionary<string, string[]> { ["transactionuid"] = uid };


            var response = await client.PostAsync(Model.Constants.MoneyarUrl.cashouttransactionreport, SRL.Json.CreateJsonBody(inp));


            var result = await HandleResponse<CashoutTransactionReportResponse>(response);

            return result;
        }
        public async Task<MoneyarResponse<MoneyarReverseResponse>> Reverse(string uid)
        {
            Dictionary<string, string> inp = new Dictionary<string, string> { ["uid"] = uid };
            var response = await client.PostAsync(Model.Constants.MoneyarUrl.reverse, SRL.Json.CreateJsonBody(inp));
            var result = await HandleResponse<MoneyarReverseResponse>(response);
            return result;
        }





        private async Task<MoneyarResponse<T>> HandleResponse<T>(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode) return new MoneyarResponse<T> { code = (int)response.StatusCode };

            var result = await response.Content.ReadAsAsync<MoneyarResponse<T>>();

            return result;
        }
        public void ThrowError()
        {
            throw new GlobalException(ErrorCode.FailedDependency);
        }

        public void ThrowIfError<T>(MoneyarResponse<T> response)
        {
            if (!response.IsOk) ThrowError();
        }













    }


}
