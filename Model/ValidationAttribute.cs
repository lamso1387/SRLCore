using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace SRLCore.Model
{
    public class ValidationAttr
    {
        public class MobileAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                if (value == null) return false;
                string value_ = value.ToString();
                if (string.IsNullOrWhiteSpace(value_)) return false;
                if (value_.Length != 11) return false;
                if (value_.Substring(0, 1) != "0") return false;
                string mobile = value_.Substring(1, 10);
                long m;
                if (!long.TryParse(mobile, out m)) return false;
                return true;
            }
        }
        public class MobileNullableAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                if (value == null) return true;
                string value_ = value.ToString();
                if (string.IsNullOrWhiteSpace(value_)) return true;
                if (value_.Length != 11) return false;
                if (value_.Substring(0, 1) != "0") return false;
                string mobile = value_.Substring(1, 10);
                long m;
                if (!long.TryParse(mobile, out m)) return false;
                return true;
            }
        }
        public class ShabaAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                if (value == null) return false;
                string value_ = value.ToString();
                if (string.IsNullOrWhiteSpace(value_)) return false;
                if (value_.Length != 26) return false;
                if (value_.Substring(0, 2) != "IR") return false;
                string shaba = value_.Substring(2, 24);
                var regex = new Regex(@"^\d+$");
                if (!regex.IsMatch(shaba))
                    return false;
                return true;
            }
        }
        public class ShabaNullableAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                if (value == null) return true;
                string value_ = value.ToString();
                if (string.IsNullOrWhiteSpace(value_)) return true;
                if (value_.Length != 26) return false;
                if (value_.Substring(0, 2) != "IR") return false;
                string shaba = value_.Substring(2, 24);

                if (!Tools.IdentifierTools.IsNumber(shaba)) return false;
                return true;
            }
        }
        public class NationalCodeAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                if (value == null) return true;
                string value_ = value.ToString();
                if (string.IsNullOrWhiteSpace(value_)) return true;
                if (value_.Length != 10) return false;
                long n;
                if (!long.TryParse(value_, out n)) return false;
                return true;
            }
        }
        public class CoOrNationalCodeAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                if (value == null) return true;
                string value_ = value.ToString();
                if (string.IsNullOrWhiteSpace(value_)) return true;
                if (value_.Length != 10 || value_.Length != 11) return false;
                long n;
                if (!long.TryParse(value_, out n)) return false;
                return true;
            }
        }
        public class CoNationalCodeAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                if (value == null) return false;
                long value_;
                if (!long.TryParse(value.ToString(), out value_)) return false;
                if (value_.ToString().Length != 11) return false;
                return true;
            }
        }
        public class PasswordAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                if (value == null) return false;
                string pass = value.ToString();
                if (pass.Length < 8) return false;
                return true;
            }
        }
        public class PersianDateAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                if (value == null) return false;
                if (value.ToString().Length != 10) return false;

                Regex r = new Regex(@"^1[34][0-9][0-9]\/((1[0-2])|(0[1-9]))\/(([12][0-9])|(3[01])|0[1-9])");

                if (r.Match(value.ToString()).Success) return true;
                else return false;

            }
        }
        public class PersianDateNullableAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                if (value == null) return true;
                if (string.IsNullOrWhiteSpace(value.ToString())) return true;

                if (value.ToString().Length != 10) return false;

                Regex r = new Regex(@"^1[34][0-9][0-9]\/((1[0-2])|(0[1-9]))\/(([12][0-9])|(3[01])|0[1-9])");

                if (r.Match(value.ToString()).Success) return true;
                else return false;

            }
        }
        public class TimeAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                if (value == null) return false;
                if (value.ToString().Length != 8) return false;
                Regex r = new Regex(@"^(((0[0-9])|(1[0-9])|(2[0-3])):([0-5][0-9]):[0-5][0-9])");

                if (r.Match(value.ToString()).Success) return true;
                else return false;

            }
        }
        public class TimeNullableAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                if (value == null) return true;
                if (string.IsNullOrWhiteSpace(value.ToString())) return true;

                if (value.ToString().Length != 8) return false;
                Regex r = new Regex(@"^(((0[0-9])|(1[0-9])|(2[1-3])):([0-5][0-9]):[0-5][0-9])");

                if (r.Match(value.ToString()).Success) return true;
                else return false;

            }
        }
        public class OfficialBarnameNoAttribute : ValidationAttribute
        {
            public static List<string> valid_regex_list = new List<string> { "14[0-9][0-9]/[0-9][0-9]-[0-9][0-9][0-9][0-9][0-9][0-9]",
            "14[0-9][0-9]/[0-9][0-9]-[0-9][0-9][0-9][0-9][0-9][0-9][0-9]" };
            public override bool IsValid(object value)
            {
                if (value == null) return false;
                if (string.IsNullOrWhiteSpace(value.ToString())) return false;

                foreach (var valid_regex in valid_regex_list)
                {
                    Regex r = new Regex(valid_regex);
                    if (r.Match(value.ToString()).Success) return true;
                }

                return false;
            }
        }
        public class Number : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                if (value == null) return false;
                if (string.IsNullOrWhiteSpace(value.ToString())) return false;
                long value_;
                if (!long.TryParse(value.ToString(), out value_)) return false;
                else return true;

            }
        }
        public class NumberNullable : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                if (value == null) return true;
                if (string.IsNullOrWhiteSpace(value.ToString())) return true;
                long value_;
                if (!long.TryParse(value.ToString(), out value_)) return false;
                else return true;
            }
        }

        public class DateRangeAttribute : RangeAttribute
        {
            public DateRangeAttribute()
               : base(typeof(DateTime), DateTime.Now.AddYears(-20).ToShortDateString(), DateTime.Now.AddYears(20).ToShortDateString()) { }
        }
    }
}
