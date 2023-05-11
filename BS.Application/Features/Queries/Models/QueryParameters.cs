using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Application.Features.Queries.Models
{
    public class QueryParameters
    {
        private const int _maxSize = 100;
        private int _pageSize = 20;

        public int Page { get; set; } = 1;

        public int Size
        {
            get => _pageSize;
            set { _pageSize = Math.Min(_maxSize, value); }
        }


        public int? AccountNumber { get; set; } = null;
    }
}
