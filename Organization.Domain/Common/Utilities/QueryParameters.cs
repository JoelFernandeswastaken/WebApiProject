using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Domain.Common.Utilities
{
    public class QueryParameters
    {
        private int _maxPageSize = 100;
        private int _pageSize = 100;
        private string _sortOrder = "asc";
        private string _sortBy = "PagingOrder";

        public int PageNo { get; set; } = 1;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = Math.Min(_maxPageSize, value);
            }
        }
        public string SortOrder
        {
            get
            {
                return _sortOrder;
            }
            set
            {
                if(value == "asc" || value == "desc")
                    _sortOrder = value;
            }
        }
        public string SortBy
        {
            get
            {
                return _sortBy;
            }
            set
            {
                if (value == "PagingOrder")
                    _sortBy = value;
            }
        }
    }
    

}
