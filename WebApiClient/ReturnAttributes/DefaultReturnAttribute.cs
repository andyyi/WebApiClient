﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebApiClient
{
    /// <summary>
    /// 表示将回复的结果作HttpResponseMessage或byte[]或string处理
    /// 如果使用其它类型作接收结果，将引发NotSupportedException
    /// 这是框架默认采用的回复解析特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class DefaultReturnAttribute : ApiReturnAttribute
    {
        /// <summary>
        /// 异步获取结果
        /// </summary>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public override async Task<object> GetResultAsync(ApiActionContext context)
        {
            var response = await context.ResponseMessage;
            var returnType = context.ApiActionDescriptor.ReturnDataType;

            if (returnType == typeof(HttpResponseMessage))
            {
                return response;
            }

            if (returnType == typeof(byte[]))
            {
                return await response.Content.ReadAsByteArrayAsync();
            }

            if (returnType == typeof(string))
            {
                return await response.Content.ReadAsStringAsync();
            }

            var message = string.Format("不支持的类型{0}的解析", returnType);
            throw new NotSupportedException(message);
        }
    }
}
