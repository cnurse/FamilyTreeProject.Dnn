//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Naif.Core.Caching;
using Naif.Core.Collections;
using Naif.Data;

namespace FamilyTreeProject.Dnn.Data
{
    public class DnnRepository<TModel> : RepositoryBase<TModel> where TModel : class
    {
        private readonly DotNetNuke.Data.IRepository<TModel> _repository;

        public DnnRepository(DotNetNuke.Data.IRepository<TModel> repository, ICacheProvider cache) : base(cache)
        {
            _repository = repository;
        }

        public string PrimaryKey { get; set; }

        public override IEnumerable<TModel> Find(string sqlCondition, params object[] args)
        {
            return _repository.Find(sqlCondition, args);
        }

        public override IPagedList<TModel> Find(int pageIndex, int pageSize, string sqlCondition, params object[] args)
        {
            return Find(sqlCondition, args).InPagesOf(pageSize).GetPage(pageIndex);
        }

        public override IEnumerable<TModel> Find(Expression<Func<TModel, bool>> predicate)
        {
            return _repository.Get().AsQueryable().Where(predicate);
        }

        public override IPagedList<TModel> Find(int pageIndex, int pageSize, Expression<Func<TModel, bool>> predicate)
        {
            return Find(predicate).InPagesOf(pageSize).GetPage(pageIndex);
        }

        protected override void AddInternal(TModel item)
        {
            _repository.Insert(item);
        }

        protected override void DeleteInternal(TModel item)
        {
            _repository.Delete(item);
        }

        protected override IEnumerable<TModel> GetAllInternal()
        {
            return _repository.Get();
        }

        protected override TModel GetByIdInternal(object id)
        {
            return _repository.Find(GetWhereClause(PrimaryKey), id).SingleOrDefault();
        }

        protected override IEnumerable<TModel> GetByScopeInternal(object scopeValue)
        {
            return _repository.Find(GetWhereClause(Scope), scopeValue);
        }

        protected override IPagedList<TModel> GetPageByScopeInternal(object scopeValue, int pageIndex, int pageSize)
        {
            return GetByScopeInternal(scopeValue).InPagesOf(pageSize).GetPage(pageSize);
        }

        protected override IPagedList<TModel> GetPageInternal(int pageIndex, int pageSize)
        {
            return GetAllInternal().InPagesOf(pageSize).GetPage(pageSize);
        }

        protected override void UpdateInternal(TModel item)
        {
            _repository.Update(item);
        }

        private string GetWhereClause(string columnName)
        {
            return String.Format("WHERE {0} = @0", columnName);
        }

        protected override IEnumerable<TModel> GetByPropertyInternal<TProperty>(string propertyName, TProperty propertyValue)
        {
            throw new NotImplementedException();
        }
    }
}