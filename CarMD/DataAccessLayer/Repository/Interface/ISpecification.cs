 using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.Interface
{
    public interface ISpecification<E>
    {
        /// <summary>
        /// Select/Where Expression
        /// </summary>
        Expression<Func<E, bool>> EvalPredicate { get; }
        /// <summary>
        /// Function to evaluate where Expression
        /// </summary>
        Func<E, bool> EvalFunc { get; }
    }
}
