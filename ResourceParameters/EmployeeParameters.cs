using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystemLoginHr.ResourceParameters
{
    public class EmployeeParameters
    {
        public string? Name { get; set; }
        public string? JobTitle { get; set; }
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }

        //pagination
        const int maxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}
