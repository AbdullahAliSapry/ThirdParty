using Infrastructure.Utlities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAl.IRepository
{
    public interface IBaseRepositrory<T> where T : class
    {

        StatusOpertion Create(T item);
        
        Task<StatusOpertion> CreateRange(List<T> items);
        
        List<T> GetItems(bool getRel = true);

        Task<List<T>> UpdateAsync(List<T> values);

    
    }

}
