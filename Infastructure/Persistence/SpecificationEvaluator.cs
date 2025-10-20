using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Contracts;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    static class SpecificationEvaluator
    {
        // Create Query
        // _dbContext.Set<TEntity>().Where(P => P.Id == id ).Include(P => P.ProductBrand).Include(P => P.ProductBrand)
        public static IQueryable<TEntity> CreateQuery<TEntity,TKey>(IQueryable<TEntity> inputQuery , ISpecification<TEntity,TKey> specification) where TEntity : BaseEntity<TKey>
        {
            //build query
            //_dbContext.Set<TEntity>()
            var query = inputQuery;
            if(specification.Criteria is not null)
            {
                //_dbContext.Set<TEntity>().Where(P => P.Id == id )
                query = query.Where(specification.Criteria);
            }
            if(specification.OrderBy is not null)
            {
                query = query.OrderBy(specification.OrderBy);
            }
            if(specification.OrderByDescending is not null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }
            if (specification.IsPagination)
            {
                query = query.Skip(specification.Skip);
                query = query.Take(specification.Take);
            }
            if(specification.IncludeExpressions is not null && specification.IncludeExpressions.Any())
            {
                //foreach(var expression in specification.IncludeExpressions)
                //{
                //    query = query.Include(expression);
                //}
                // _dbContext.Set<TEntity>().Where(P => P.Id == id ).Include(P => P.ProductBrand).Include(P => P.ProductBrand)

                query = specification.IncludeExpressions.Aggregate(query,(currentQuery,includeExp) => currentQuery.Include(includeExp));
            }
            return query;
        }
    }
}
