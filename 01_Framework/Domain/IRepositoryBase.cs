using System.Linq.Expressions;

namespace _01_Framework.Domain
{
    public interface IRepositoryBase<TKey, T> where T : class
    {
        void Create(T entity);
        T GetBy(TKey id);
        bool Exists(Expression<Func<T, bool>> expression);
        void Save();
    }
}
