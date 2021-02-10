using FootballLeagueAPI.DBContexts;
using FootballLeagueAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FootballLeagueAPI.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected SqlDbContext SqlDbContext { get; set; }
        public RepositoryBase(SqlDbContext context)
        {
            SqlDbContext = context;
        }
        public IQueryable<T> GetAll()
        {
            return SqlDbContext.Set<T>();
        }
        public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression)
        {
            return SqlDbContext.Set<T>().Where(expression);
        }
        public void Create(T entity)
        {
            SqlDbContext.Set<T>().Add(entity);
        }
        public void Update(T entity)
        {
            SqlDbContext.Set<T>().Update(entity);
        }
        public void Delete(T entity)
        {
            SqlDbContext.Set<T>().Remove(entity);
        }
    }
}