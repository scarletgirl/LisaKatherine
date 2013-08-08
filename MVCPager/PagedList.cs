/*
 ASP.NET MvcPager control
 Copyright:2009-2011 Webdiyer (http://en.webdiyer.com)
 Source code released under Ms-PL license
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace Webdiyer.WebControls.Mvc
{
    public class PagedList<T> : List<T>,IPagedList
    {
        public PagedList(IList<T> items,int pageIndex,int pageSize)
        {
            PageSize = pageSize;
            TotalItemCount = items.Count;
            TotalPageCount = (int)Math.Ceiling(TotalItemCount / (double)PageSize);
            CurrentPageIndex = pageIndex;
            StartRecordIndex=(CurrentPageIndex - 1) * PageSize + 1;
            EndRecordIndex = TotalItemCount > pageIndex * pageSize ? pageIndex * pageSize : TotalItemCount;
            for (int i = StartRecordIndex-1; i < EndRecordIndex;i++ )
            {
                Add(items[i]);
            }
        }

        protected readonly List<T> Subset = new List<T>();

        public PagedList(IEnumerable<T> items, int pageIndex, int pageSize, int totalItemCount)
        {
            //AddRange(items);
            TotalItemCount = totalItemCount;
            TotalPageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize);
            CurrentPageIndex = pageIndex;
            PageSize = pageSize;
            StartRecordIndex = (pageIndex - 1) * pageSize + 1;
            EndRecordIndex = TotalItemCount > pageIndex * pageSize ? pageIndex * pageSize : totalItemCount;
            if (items != null && TotalItemCount > 0)
                Subset.AddRange(pageIndex == 1
                    ? items.Skip(0).Take(pageSize).ToList()
                    : items.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList()
                );
            AddRange(Subset);
        }

        public int CurrentPageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalItemCount { get; set; }
        public int TotalPageCount{get; private set;}
        public int StartRecordIndex{get; private set;}
        public int EndRecordIndex{get; private set;}
    }
}
