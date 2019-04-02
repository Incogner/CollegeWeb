using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace College.Models.ViewModels
{
    public class CourseListViewModel
    {
        public IEnumerable<Course> Courses { get; set; }
        public PagingInfo PagingInfo { get; set; }
        // ********** SEARCH FEATURE **********
        public string SearchString { get; set; }
        public string SearchField { get; set; }
        // ********** SEARCH FEATURE **********

    }
}
