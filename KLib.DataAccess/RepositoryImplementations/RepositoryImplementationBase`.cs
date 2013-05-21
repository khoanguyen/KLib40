using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace KLib.DataAccess.RepositoryImplementations
{
    internal abstract class RepositoryImplementationBase<TEntity>
    {
        protected IEnumerable<string> GetChangeSet(Expression<Func<TEntity, object>>[] changeSet)
        {
            return changeSet.Select(selector => GetMemberName(selector));
        }

        private string GetMemberName(Expression<Func<TEntity, object>> selector)
        {
            var body = selector.Body as MemberExpression;
            if (body == null)
            {
                var convert = selector.Body as UnaryExpression;
                body = convert.Operand as MemberExpression;
            }

            return body.Member.Name;
        }
    }
}
