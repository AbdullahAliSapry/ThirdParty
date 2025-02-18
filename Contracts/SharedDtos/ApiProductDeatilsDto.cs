using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.SharedDtos
{
    public class ApiProductDeatilsDto<T> : ApiMainInfo where T : class
    {


        public ResultProductDto<T> Result { get; set; }

    }

    public class ResultProductDto<T>
    {
        public T Item { get; set; }
    }
}
