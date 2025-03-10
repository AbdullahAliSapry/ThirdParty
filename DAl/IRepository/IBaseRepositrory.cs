using Infrastructure.Utlities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        bool UpdateItem(T value);
        Task<T> GetItem(string GetItem, string[] includes=null) ;
        List<T> GetItemsWithFunc(Expression<Func<T, bool>> expression, string[] includes = null);

        Task<T> GetItemWithFunc(Expression<Func<T, bool>> expression, string[] includes = null);


        Task<bool> DeleteItemWithFunc(Expression<Func<T, bool>> expression);
        Task<bool> DeleteRangeAsync(Expression<Func<T, bool>> expression);

    }

}
