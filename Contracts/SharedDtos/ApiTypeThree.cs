using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.SharedDtos
{
    public class ApiTypeThree<T>:ApiMainInfo where T : class
    {

    

        public OtapiItemInfoSubList<T> OtapiItemInfoSubList { get; set; }


    }
    public class OtapiItemInfoSubList<T>
    {

        public List<T> Content { get; set; }

    }
}
