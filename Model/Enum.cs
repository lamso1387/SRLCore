using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Net;

namespace SRLCore.Model
{
    public enum PassMode
    {
        none = 0,
        add = 1,
        edit = 2
    }
    public enum ViewMode
    {
        ReadOnly = 0,
        Edit = 1,
        Insert = 2,
        ExcelInsert = 3
    }
    public enum EntityStatus
    {
        inactive = 0,
        active = 1
    }
    public enum EventType
    {
        Call = 10,
        Return = 11,
        Exception = 12,
        Operation = 13
    }

    public enum ErrorCode
    {
        [Description(@"{""message"":""عملیات با موفقیت انجام شد"" ,""status"": ""OK""}")]
        OK = HttpStatusCode.OK,
        [Description(@"{""message"":""ورودی اشتباه است"" ,""status"": ""BadRequest""}")]
        BadRequest =HttpStatusCode.BadRequest,
        [Description(@"{""message"":""خطای غیرمنتظره رخ داده است لطفا مجددا تلاش کنید یا با پشتیبان تماس بگیرید"" ,""status"": ""ExpectationFailed""}")]
        UnexpectedError = HttpStatusCode.ExpectationFailed,
        [Description(@"{""message"":""اطلاعات ذخیره نشد مجددا تلاش کنید یا با پشتیبان تماس بگیرید"" ,""status"": ""ExpectationFailed""}")]
        DbSaveNotDone = 3,
        [Description(@"{""message"":""خطا در ذخیره سازی اطلاعات رخ داد  با پشتیبان تماس بگیرید"" ,""status"": ""UnprocessableEntity""}")]
        DbUpdateException = 4,
        [Description(@"{""message"":""اطلاعات تکراری است"" ,""status"": ""Conflict""}")]
        AddRepeatedEntity = 5,
        [Description(@"{""message"":""مورد یافت نشد"" ,""status"": ""NoContent""}")]
        NoContent = HttpStatusCode.NoContent,
        [Description(@"{""message"":""مورد یافت نشد"" ,""status"": ""BadRequest""}")]
        ItemNotExists = 499,
        [Description(@"{""message"":""هزینه ابطال صندوق تعیین نشده است"" ,""status"": ""PreconditionFailed""}")]
        FundCancelCostNotSet = 7,
        [Description(@"{""message"":""سود سالانه تعیین نشده است"" ,""status"": ""PreconditionFailed""}")]
        AnnualProfitNotSet = 8,
        [Description(@"{""message"":""دسترسی به اطلاعات وجود ندارد"" ,""status"": ""Forbidden""}")]
        Forbidden = HttpStatusCode.Forbidden,
        [Description(@"{""message"":""نام کاربری یا رمز عبور اشتباه است"" ,""status"": ""Unauthorized""}")]
        Unauthorized = HttpStatusCode.Unauthorized,
        [Description(@"{""message"":""خطا در سرویس وابسته رخ داد"" ,""status"": ""FailedDependency""}")]
        FailedDependency = HttpStatusCode.FailedDependency,
        [Description(@"{""message"":""شرایط لازم برای انجام این عملیات رعایت نشده است"" ,""status"": ""PreconditionFailed""}")]
        PreconditionFailed = HttpStatusCode.PreconditionFailed,
        [Description(@"{""message"":""مغایرت در انجام عملیات"" ,""status"": ""Conflict""}")]
        Conflict = HttpStatusCode.Conflict,
        [Description(@"{""message"":""خطا در انجام عملیات رخ داده است با پشتیبان تماس بگیرید"" ,""status"": ""UnprocessableEntity""}")]
        InvalidOperationException = HttpStatusCode.UnprocessableEntity,
        [Description(@"{""message"":""ورودی بدرستی تنظیم نشده است"" ,""status"": ""Gone""}")]
        InvalidRequest = HttpStatusCode.Gone,
        [Description(@"{""message"":""اطلاعات ثبت شد ولی خطایی در ادامه عملیات رخ داد"" ,""status"": ""Created""}")]
        Created = HttpStatusCode.Created
    }
    public enum NonActionAccess
    {
        //[Display(Name = "اطلاعات شخصی")]
        //MyData=0, 

    }

    public enum AppLogType
    {
        start=0,
        end=1

    }
    public enum RequestType
    {
        add = 0,
        edit = 1

    }

    public enum DotNetCoreVersion
    {
        core2_1 = 0,
        core5_0 = 1

    }


    public class EnumConvert
    {
        public static T StringToEnum<T>(string str)
        {
            return SRL.Convertor.StringToEnum<T>(str);
        }

    }

}
