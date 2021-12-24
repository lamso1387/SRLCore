
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; 

namespace SRLCore
{
    public class Constants
    {
        public class MessageText
        {
            public const string RequiredFieldError = "فیلد اجباری تکمیل نشده است";
            public const string RangeFieldError = "مقدار وارد شده معتبر نیست";
            public const string RangeFieldErrorDynamic = "مقدار وارد شده فیلد {0} معتبر نیست";
            public const string AdditionalParameter = "ورودی دارای اطلاعات اضافی است";
            public const string RequiredFieldErrorDynamic = "فیلد اجباری {0} تکمیل نشده است";
            public const string ErrorNotSet = "خطا تعیین نشده است";
            public const string AddRepeatedField = "مقادیر فیلد ورودی باید متمایز باشد";
            public const string FieldFormatErrorDynamic = "فرمت {0} اشتباه است";
            public const string PasswordFormatError = "فرمت رمز عبور اشتباه است";
            public const string PurcheseAmountSumError = "جمع مبالغ سرمایه گذاران باید برابر مبلغ سرمایه گذاری باشد";
            public const string ContractorsEqualError = "طرفین قرارداد نمی توانند یکسان باشند";
            public const string RoleAccessNotDefinedError = "دسترسی ها مشخص نشده است";
            public const string RoleUsersNotDefinedError = "کاربران مشخص نشده اند";
            public const string NoDataAccess = "دسترسی به اطلاعات وجود ندارد";
            public static string RequiredFieldErrorMes(string field) =>  $"{RequiredFieldError}: {field}";
        } 
        public class Singleton
        {
            private Singleton()
            {
            }
            private static readonly Lazy<Singleton> lazy = new Lazy<Singleton>(() => new Singleton());
            public static Singleton Instance
            {
                get
                {
                    return lazy.Value;
                }
            } 
        }
    }
}
