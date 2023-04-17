using SRLCore.Middleware;
using SRLCore.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic.Core;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json; 
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using static SRLCore.Model.ValidationAttr;

namespace SRLCore.Model
{
   

    public abstract class WebRequest
    {
        public long? id { get; set; } 
        public void CheckValidation()
        { 
            if (CheckAttrbuteValidation())
                if (CheckPropertyValidation())
                {
                    
                }
            if (validation_errors.Any())
                throw new GlobalException(ErrorCode.BadRequest, validation_errors.First());

        }
        public bool CheckAttrbuteValidation()
        {
            validation_errors = SRL.ClassManagement.CheckValidationAttribute(this);
            return validation_errors.Count == 0 ? true : false;
        }
        protected List<string> validation_errors { get; set; }
        protected virtual bool CheckPropertyValidation() { return true; }
        protected virtual bool CheckAccessValidation() { return true; }
        public virtual EntityT ToEntity2<EntityT>(long? edit_id = null)
            where EntityT : CommonProperty
        { return null; }
         
    }

    public class WebPageRequest : WebRequest
    {  
        public int page_start { get; set; } 
        public int page_size { get; set; }
    }
    public static class IQueryableExtensions
    {
        public static IQueryable<TModel> Paging<TModel>(this IQueryable<TModel> query, PagedResponse<object> response, int pageStart = 0, int pageSize = 0) where TModel : class
        {
            response.ItemsCount = query.Count();
            response.PageNumber = pageStart;
            response.PageSize = pageSize;
            return pageSize > 0 && pageStart > 0 ? query.Skip((pageStart - 1) * pageSize).Take(pageSize) :
                query.Skip(0);

        }
        public static IQueryable<T> FilterNonActionAccess<T>(this IQueryable<T> query, string my_id, Func<IQueryable<T>, IQueryable<T>> MyUnionFunc)
        {
            IQueryable<T> data_to_union = null;
            List<string> where_list = new List<string>();
            string share_id = nameof(IUser.creator_id);
            // where_list.Add($"{all_data}");

           // if (!string.IsNullOrWhiteSpace(my_id)) where_list.Add($"({my_data} and {my_id}=={UserSession.Id})");
            // else if (my_data && MyUnionFunc != null) data_to_union = MyUnionFunc(query);

            //where_list.Add($"({share_data} and { share_id}=={ UserSession.Id})");
            string where_clause = string.Join(" || ", where_list);
            if (!string.IsNullOrWhiteSpace(where_clause)) query = query.Where(where_clause).AsQueryable();
            if (data_to_union != null) query = query.Union(data_to_union).OrderBy(nameof(CommonProperty.create_date));
            return query;
        }
        public static IQueryable<EntityT> FilterConstraintById<EntityT>(this IQueryable<EntityT> query,List<string> constraints_str) where EntityT:ICommonProperty
        {
             List<long> constrains = new List<long>();
            foreach (var item in constraints_str)
            {
                if(item !=null)
                constrains.AddRange(item.Split(',').ToList().Select(y => long.Parse(y)).ToList());

            } 
            if(constrains.Any())
            {
                query = query.Where(x => constrains.Contains(x.id));
            }    
            return query;
        }
        public static IQueryable<EntityT> FilterConstraintByIntColumn<EntityT>(this IQueryable<EntityT> query, List<string> constraints_str, string col_name) where EntityT : ICommonProperty
        {
            List<string> constrains = new List<string>();
            foreach (var item in constraints_str)
            {
                if (item != null)
                    constrains.AddRange(item.Split(',').ToList());

            }
            if (constrains.Any())
            {
                List<string> where_list = new List<string>();
                foreach (var constrain in constrains)
                {
                    where_list.Add($" {col_name}=\"{constrain}\" ");
                }
                
                string where_clause = string.Join(" || ", where_list);
                if (!string.IsNullOrWhiteSpace(where_clause)) query = query.Where(where_clause).AsQueryable();
            }
            return query;
        }

        public static IQueryable<EntityT> FilterConstraintByStringColumn<EntityT>(this IQueryable<EntityT> query, List<string> constraints_str, string col_name)
        {
            List<string> constrains = new List<string>();
            foreach (var item in constraints_str)
            {
                if (item != null)
                    constrains.AddRange(item.Split(',').ToList());

            }
            if (constrains.Any())
            {
                List<string> where_list = new List<string>();
                List<object> parameters = new List<object>(); 
                foreach (var constrain in constrains)
                {  
                    where_list.Add($"{col_name} == @{constrains.IndexOf(constrain)}");
                    parameters.Add(constrain);
                }

                string where_clause = string.Join(" || ", where_list);
                if (!string.IsNullOrWhiteSpace(where_clause)) query = query.Where(where_clause,parameters.ToArray()).AsQueryable();
            }
            return query;
        }
    }


    
   
    public class PagedRequest
    {
        /// <summary>
        /// 1
        /// </summary>
        public int page_start { get; set; }
        /// <summary>
        /// 100
        /// </summary>
        public int page_size { get; set; }
    } 

   
    public class SearchUserRequest : PagedRequest
    {
        public long? id { get; set; }
        public int? status { get; set; }
    }
    public class AddUserRequest : WebRequest
    {

        internal SRLCore.Model.PassMode pass_mode { get; set; }

        [Required(ErrorMessage = Constants.MessageText.RequiredFieldErrorDynamic), DisplayName("نام")]
        public string first_name { get; set; }
        [Required(ErrorMessage = Constants.MessageText.RequiredFieldErrorDynamic), DisplayName("نام خانوادگی")]
        public string last_name { get; set; }
        [Required(ErrorMessage = Constants.MessageText.RequiredFieldErrorDynamic), DisplayName("موبایل")]
        [Mobile(ErrorMessage = Constants.MessageText.FieldFormatErrorDynamic)]
        public string mobile { get; set; }
        [Required(ErrorMessage = Constants.MessageText.RequiredFieldErrorDynamic), DisplayName("کدملی")]
        [NationalCode(ErrorMessage = Constants.MessageText.FieldFormatErrorDynamic)]
        public string national_code { get; set; }
        public string password { get; set; }
        protected override bool CheckPropertyValidation()
        {
            bool is_valid = true;
            if (pass_mode == SRLCore.Model.PassMode.add || !string.IsNullOrWhiteSpace(password))
            {
                if (password == null) is_valid = false;
                else
                {
                    string pass = password.ToString();
                    if (pass.Length < 8) is_valid = false;
                }
            }
            if (is_valid == false)
                validation_errors.Add(Constants.MessageText.PasswordFormatError);

            return is_valid;
        }



    }

    public class AddRoleRequest : WebRequest
    {
        [Required(ErrorMessage = Constants.MessageText.RequiredFieldErrorDynamic), DisplayName("نام")]
        public string name { get; set; }
        [Required(ErrorMessage = Constants.MessageText.RequiredFieldErrorDynamic), DisplayName("دسترسی ها")]
        public List<string> accesses { get; set; }
        [Required(ErrorMessage = Constants.MessageText.RequiredFieldErrorDynamic), DisplayName("کاربران")]
        public List<long> user_ids { get; set; }
        protected override bool CheckPropertyValidation()
        {
            bool is_valid = true;
            is_valid = accesses == null ? false : accesses.Count > 0;
            if (is_valid == false)
                validation_errors.Add(Constants.MessageText.RoleAccessNotDefinedError);
            else
            {
                is_valid = user_ids == null ? false : user_ids.Count > 0;
                if (is_valid == false)
                    validation_errors.Add(Constants.MessageText.RoleUsersNotDefinedError);
            }
            return is_valid;
        }


    }
}
