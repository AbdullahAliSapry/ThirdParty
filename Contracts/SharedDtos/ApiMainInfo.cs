using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.SharedDtos
{
    public class ApiMainInfo
    {

        public string ErrorCode { get; set; }
        public object SubErrorCode { get; set; }
        public string RequestId { get; set; }
        public double RequestTime { get; set; }


    }
}


