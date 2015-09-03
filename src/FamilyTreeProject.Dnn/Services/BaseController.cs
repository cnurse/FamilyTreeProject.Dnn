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
using System.Net.Http;
using DotNetNuke.Web.Api;
using Naif.Core.Collections;

namespace FamilyTreeProject.Dnn.Services
{
    public class BaseController : DnnApiController
    {
        public HttpResponseMessage GetEntity<TEntity, TViewModel>(Func<TEntity> getEntity, Func<TEntity, TViewModel> createViewModel)
        {
            var entity = getEntity();
            var viewModel = createViewModel(entity);

            var response = new
            {
                success = true,
                data = new
                    {
                        results = viewModel,
                    }
                };

            return Request.CreateResponse(response);
        }

        public HttpResponseMessage GetEntities<TEntity, TViewModel>(Func<IEnumerable<TEntity>> getEntities, Func<TEntity, TViewModel> createViewModel )
        {
            var entityList = getEntities().ToList();
            var viewModels = entityList
                // ReSharper disable once ConvertClosureToMethodGroup
                               .Select(entity => createViewModel(entity))
                               .ToList();

            var response = new
                            {
                                success = true,
                                data = new
                                        {
                                            results = viewModels,
                                            totalResults = entityList.Count()
                                        }
                            };

            return Request.CreateResponse(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="getEntities"></param>
        /// <param name="getViewModel"></param>
        /// <returns></returns>
        protected HttpResponseMessage GetPage<TEntity, TViewModel>(Func<IPagedList<TEntity>> getEntities, Func<TEntity, TViewModel> getViewModel)
        {
            var entityList = getEntities();
            var viewModels = entityList
                // ReSharper disable once ConvertClosureToMethodGroup
                                .Select(entity => getViewModel(entity))
                                .ToList();

            var response = new
                            {
                                success = true,
                                data = new
                                        {
                                            results = viewModels,
                                            totalResults = entityList.TotalCount
                                        }
                            };

            return Request.CreateResponse(response);
        }

    }
}