using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;


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
            OK = 0,
            [Description(@"{""message"":""ورودی اشتباه است"" ,""status"": ""BadRequest""}")]
            BadRequest = 1,
            [Description(@"{""message"":""خطای غیرمنتظره رخ داده است لطفا مجددا تلاش کنید یا با پشتیبان تماس بگیرید"" ,""status"": ""InternalServerError""}")]
            UnexpectedError = 2,
            [Description(@"{""message"":""اطلاعات ذخیره نشد مجددا تلاش کنید یا با پشتیبان تماس بگیرید"" ,""status"": ""ExpectationFailed""}")]
            DbSaveNotDone = 3,
            [Description(@"{""message"":""خطا در ذخیره سازی اطلاعات"" ,""status"": ""UnprocessableEntity""}")]
            DbUpdateException = 4,
            [Description(@"{""message"":""اطلاعات تکراری است"" ,""status"": ""Conflict""}")]
            AddRepeatedEntity = 5,
            [Description(@"{""message"":""مورد یافت نشد"" ,""status"": ""NoContent""}")]
            NoContent = 6,
            [Description(@"{""message"":""هزینه ابطال صندوق تعیین نشده است"" ,""status"": ""PreconditionFailed""}")]
            FundCancelCostNotSet = 7,
            [Description(@"{""message"":""سود سالانه تعیین نشده است"" ,""status"": ""PreconditionFailed""}")]
            AnnualProfitNotSet = 8,
            [Description(@"{""message"":""دسترسی به اطلاعات وجود ندارد"" ,""status"": ""Forbidden""}")]
            NoDataAccess = 9,
            [Description(@"{""message"":""نام کاربری یا رمز عبور اشتباه است"" ,""status"": ""Unauthorized""}")]
            Unauthorized = 10
        }
        public enum NonActionAccess
        {
            //[Display(Name = "اطلاعات شخصی")]
            //MyData=0, 

        }


    public class EnumConvert
    {
        public static T StringToEnum<T>(string str)
        {
            return SRL.Convertor.StringToEnum<T>(str);
        }

    }

}
