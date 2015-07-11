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
                        results = entity,
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
    }
}