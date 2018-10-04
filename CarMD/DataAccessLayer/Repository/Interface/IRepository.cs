using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.Interface
{
    public interface IRepository<E>
    {
        string KeyProperty { get; set; }

        void Add(E entity);
        void AddOrAttach(E entity);
        void DeleteRelatedEntries(E entity);
        void DeleteRelatedEntries
        (E entity, ObservableCollection<string> keyListOfIgnoreEntites);
        void Delete(E entity);

        ObjectQuery<E> DoQuery();
        ObjectQuery<E> DoQuery(ISpecification<E> where);
        ObjectQuery<E> DoQuery(int maximumRows, int startRowIndex);
        ObjectQuery<E> DoQuery(Expression<Func<E, object>> sortExpression);
        ObjectQuery<E> DoQuery(Expression<Func<E, object>> sortExpression,
                    int maximumRows, int startRowIndex);

        IList<E> SelectAll(string entitySetName);
        IList<E> SelectAll();
        IList<E> SelectAll(string entitySetName, ISpecification<E> where);
        IList<E> SelectAll(ISpecification<E> where);
        IList<E> SelectAll(int maximumRows, int startRowIndex);
        IList<E> SelectAll(Expression<Func<E, object>> sortExpression);
        IList<E> SelectAll(Expression<Func<E, object>> sortExpression,
                    int maximumRows, int startRowIndex);

        E SelectByKey(long Key);

        bool TrySameValueExist(string fieldName, object fieldValue, string key);
        bool TryEntity(ISpecification<E> selectSpec);

        int GetCount();
        int GetCount(ISpecification<E> selectSpec);
    }
}
