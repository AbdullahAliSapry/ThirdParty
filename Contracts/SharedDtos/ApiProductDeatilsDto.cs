using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.SharedDtos
{
    public class ApiProductDeatilsDto<T,B,E, C> : ApiMainInfo where T : class where B : class where E : class where C : class
    {


        public ResultProductDto<T,B,E, C> Result { get; set; }

    }

    public class ResultProductDto<T,B,E, C>
    {
        public T Item { get; set; }

        public B Vendor { get; set; }
        public E VendorItems { get; set; }

        public C ProviderReviews { get; set; }

    }
}
