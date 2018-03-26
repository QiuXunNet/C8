using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C8.Lottery.Public
{
    /// <summary>
    /// 表示Ajax操作结果 
    /// </summary>
    public class AjaxResult
    {
        public AjaxResult()
        {
            Code = 100;
            Message = "成功";
        }
        public AjaxResult(int code) 
            : this(code, null)
        {
        }

        public AjaxResult(int code, string desc)
        {
            this.Code = code;
            this.Message = desc;
        }

        public AjaxResult CopyStatus(AjaxResult result)
        {
            this.Code = result.Code;
            this.Message = result.Message;
            return this;
        }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess {
            get { return Code == 100; }
        }

        /// <summary>
        /// 获取 Ajax操作结果编码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 获取 消息内容
        /// </summary>
        public string Message { get; set; }



    }

    /// <summary>
    /// 表示Ajax操作结果 
    /// </summary>
    public class AjaxResult<T> : AjaxResult
    {
        public AjaxResult()
        {
        }

        public AjaxResult(int code) : 
            base(code, null)
        {
        }

        public AjaxResult(int code, string desc) : 
            base(code, desc)
        {
        }
        /// <summary>
        /// 获取 返回数据
        /// </summary>
        public T Data { get; set; }

        public AjaxResult<T> CopyStatus(AjaxResult result)
        {
            base.Code = result.Code;
            base.Message = result.Message;
            return (AjaxResult<T>)this;
        }
    }

}
