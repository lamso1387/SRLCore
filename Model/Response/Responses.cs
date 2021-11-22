using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore; 
using Microsoft.Extensions.Caching.Distributed;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;  
using System.Net.Http;
using System.Security.AccessControl;
using Microsoft.AspNetCore.Rewrite.Internal.UrlActions;
using System.Text; 
using System.Runtime.CompilerServices;

namespace SRLCore.Model
{
    public class MessageResponse : IResponse
    {

        public int? ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
        public string ErrorDetail { get; set; }
        public string ErrorData { get; set; }
    }
    public class SingleResponse<TModel> : ISingleResponse<TModel>
    {

        public int? ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
        public string ErrorDetail { get; set; }
        public string ErrorData { get; set; }

        public TModel Model { get; set; }
    }

    public class ListResponse<TModel> : IListResponse<TModel>
    {
        public int? ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
        public string ErrorDetail { get; set; }
        public string ErrorData { get; set; }

        public IEnumerable<TModel> Model { get; set; }
    }

    public class PagedResponse<TModel> : IPagedResponse<TModel>
    {

        public int? ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
        public string ErrorDetail { get; set; }
        public string ErrorData { get; set; }

        public IEnumerable<TModel> Model { get; set; }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public int ItemsCount { get; set; }

        public int PageCount
        {
            get
            {
                if (PageSize == 0) return 0;
                return ItemsCount < PageSize ? 1 : (int)(((double)ItemsCount / PageSize) + 1);
            }
        }
    }

    public class SelectableField
    {
        //public static Func<Applicant, object> ApplicantSelector => x => new
        //{
        //    x.id,
        //    x.create_date,
        //    x.creator_id,
        //    x.modifier_id,
        //    x.modify_date,
        //    x.address,
        //    x.ceo,
        //    x.city_id,
        //    x.city.province_id,
        //    x.name,
        //    x.phone,
        //    x.related_category_id,
        //    x.representative,
        //    x.type_id,
        //    x.type_title

        //};
        //public static Func<Plan, object> PlantSelector => x => new
        //{
        //    x.id,
        //    x.create_date,
        //    x.creator_id,
        //    x.assessor_position_id,
        //    x.assessor_position_title,
        //    x.applicant_company?.city_id,
        //    x.applicant_id,
        //    x.city_title,
        //    x.province_id,
        //    x.province_title,
        //    x.estimate_riali,
        //    x.evaluation_unit_id,
        //    x.evaluation_unit_title,
        //    x.expert_id,
        //    x.expert_title,
        //    x.knowledge_based_company_type_id,
        //    x.knowledge_based_company_type_title,
        //    x.lifecycle_id,
        //    x.lifecycle_title,
        //    x.modifier_id,
        //    x.modify_date,
        //    x.referral_pdate,
        //    x.subject,
        //    x.technology_id,
        //    x.technology_title,
        //    x.type_id,
        //    x.type_title,
        //    x.shenase,
        //    events = x.events.Select(y => new
        //    {
        //        y.assessor_description,
        //        y.create_date,
        //        y.creator_id,
        //        y.description,
        //        y.doer,
        //        y.id,
        //        y.letter_number,
        //        y.letter_pdate,
        //        y.level_event_id,
        //        y.level_event_title,
        //        y.level_event.level_id,
        //        y.level_event.level_title,
        //        y.pdate,
        //        y.plan_id
        //    }).ToList()
        //};
        //public static Func<PlanEvent, object> PlantEventSelector => x => new
        //{
        //    x.id,
        //    x.create_date,
        //    x.creator_id,
        //    x.assessor_description,
        //    x.description,
        //    x.doer,
        //    x.letter_number,
        //    x.letter_pdate,
        //    x.level_event_id,
        //    x.pdate,
        //    x.plan_id,
        //    x.level_event.level_id,
        //    x.level_event.level_title,
        //    level_event_title = x.level_event.title,
        //    plan = new { x.plan.subject, x.plan.shenase }
        //};
        //public static Func<CommonProperty, object> CommonPropertySelector => x => new
        //{
        //    x.id,
        //    x.create_date,
        //    x.creator_id,
        //    x.modifier_id,
        //    x.modify_date
        //};


#pragma warning restore CS1591

    }
}