using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Web.MVC
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int MaxPages { get; private set; } = 5;
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }
        public int StartIndex { get; private set; }
        public int EndIndex { get; private set; }
        public IEnumerable<int> Pages { get; private set; }


        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            // ensure current page isn't out of range
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            else if (pageIndex > TotalPages)
            {
                pageIndex = TotalPages;
            }

            int startPage, endPage;
            if (TotalPages <= MaxPages)
            {
                // total pages less than max so show all pages
                startPage = 1;
                endPage = TotalPages;
            }
            else
            {
                // total pages more than max so calculate start and end pages
                var maxPagesBeforeCurrentPage = (int)Math.Floor((decimal)MaxPages / (decimal)2);
                var maxPagesAfterCurrentPage = (int)Math.Ceiling((decimal)MaxPages / (decimal)2) - 1;
                if (pageIndex <= maxPagesBeforeCurrentPage)
                {
                    // current page near the start
                    startPage = 1;
                    endPage = MaxPages;
                }
                else if (pageIndex + maxPagesAfterCurrentPage >= TotalPages)
                {
                    // current page near the end
                    startPage = TotalPages - MaxPages + 1;
                    endPage = TotalPages;
                }
                else
                {
                    // current page somewhere in the middle
                    startPage = pageIndex - maxPagesBeforeCurrentPage;
                    endPage = pageIndex + maxPagesAfterCurrentPage;
                }
            }

            // calculate start and end item indexes
            var startIndex = (pageIndex - 1) * pageSize;
            var endIndex = Math.Min(startIndex + pageSize - 1, count - 1);

            // create an array of pages that can be looped over
            var pages = Enumerable.Range(startPage, (endPage + 1) - startPage);

            // update object instance with all pager properties required by the view
            PageIndex = pageIndex;
            PageSize = pageSize;
            StartPage = startPage;
            EndPage = endPage;
            StartIndex = startIndex;
            EndIndex = endIndex;
            Pages = pages;


            this.AddRange(items);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }
    }
}