using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C8.Lottery.Public
{
    public class PagedList<T> 
    {
        public PagedList()
        {
            
        }

        public PagedList(int pageIndex,int pageSize,int totalCount,List<T> pageData )
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.totalCount = totalCount;
            this.PageData = pageData;
        }
        private int totalCount;

        public List<T> PageData { get; set; }
        public object ExtraData { get; set; }

        public bool HasNextPage
        {
            get
            {
                return ((this.PageIndex + 1) < this.TotalPages);
            }
        }

        public bool HasPreviousPage
        {
            get
            {
                return (this.PageIndex > 1);
            }
        }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalCount
        {
            get
            {
                return this.totalCount;
            }
            set
            {
                this.totalCount = value;
                this.TotalPages = this.TotalCount / this.PageSize;
                if ((this.TotalCount % this.PageSize) > 0)
                {
                    this.TotalPages++;
                }
            }
        }

        public int TotalPages { get; private set; }
    }
}
