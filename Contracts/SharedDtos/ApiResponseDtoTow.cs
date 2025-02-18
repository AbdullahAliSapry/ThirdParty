using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.SharedDtos
{
    public class ApiResponseDtoTow<T> : ApiMainInfo where T : class
    {
        public ResultDto<T> Result { get; set; }

    }

    public class ResultDto<T>
    {
        public ItemsDto<T> Items { get; set; }
    }

    public class ItemsDto<T>
    {

        public ItemsProduct<T> Items { get; set; }

        public string Provider { get; set; }

        public string SearchMethod { get; set; }

        public string CurrentSort { get; set; }

        public CategoriesDto Categories { get; set; }

        public int CurrentFrameSize { get; set; }

        public int MaximumPageCount { get; set; }


    }
    public class CategoriesDto
    {


        public List<CategoryDto> Content { get; set; }

    }
    public class ItemsProduct<T>
    {
        public List<T> Content { get; set; }

        public int TotalCount { get; set; }

    }







}
