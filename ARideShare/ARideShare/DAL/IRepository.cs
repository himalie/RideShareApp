﻿using ARideShare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ARideShare.DAL
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Get(
           Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           string includeProperties = "");

        //IEnumerable<User> GetAllUsers();
        TEntity GetByID(int id);
        void Insert(TEntity entity);
        //bool IsValid(string username, string password);
        void Update(TEntity entity);
        void Delete(int id);
        void Delete(TEntity id);
    }
}