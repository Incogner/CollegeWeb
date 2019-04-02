using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace College.Models.ViewModels
{
    public class StudentListViewModel
    {
        public IEnumerable<Student> Students { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
