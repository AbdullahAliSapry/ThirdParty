using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Contracts.SharedDtos
{
    public class ApiResponse<T>: ApiMainInfo where T : class
    {

        public CategoryInfoList<T> CategoryInfoList { get; set; }



    }

    public class CategoryInfoList<T> where T : class
    {
        public List<T> Content { get; set; }
    }
}


