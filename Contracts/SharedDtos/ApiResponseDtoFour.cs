using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.SharedDtos
{
    public class ApiResponseDtoFour<T> : ApiMainInfo where T : class
    {

        public CategoryInfoList<T> OtapiItemInfoList { get; set; }

    }
}
