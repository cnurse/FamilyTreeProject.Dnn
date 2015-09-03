//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using FamilyTreeProject.Dnn.Common;
using FamilyTreeProject.Dnn.ViewModels;
using FamilyTreeProject.DomainServices;
using Naif.Core.Collections;

namespace FamilyTreeProject.Dnn.Services
{
    [AllowAnonymous]
    public class IndividualController : BaseController
    {
        private readonly IIndividualService _individualService;
        private readonly IFactService _factService;

        public IndividualController()
        {
            var cache = Util.CreateCacheProvider();
            var unitOfWork = Util.CreateUnitOfWork(cache);
            var serviceFactory = new FamilyTreeServiceFactory(unitOfWork, cache);
            _individualService = serviceFactory.CreateIndividualService();
            _factService = serviceFactory.CreateFactService();
        }

        [HttpGet]
        public HttpResponseMessage GetIndividual(int treeId, int id, int includeAncestors = 0, int includeDescendants = 0, bool includeSpouses = false)
        {
            return GetEntity(() => _individualService.Get( id, treeId)
                                    , ind => new IndividualViewModel(ind, _factService, includeAncestors, includeDescendants, includeSpouses));
        }

        [HttpGet]
        public HttpResponseMessage GetIndividuals(int treeId, string searchTerm, int pageIndex, int pageSize)
        {
            Func<Individual, bool> predicate = (ind => (String.IsNullOrEmpty(searchTerm)) || ind.Name.ToLowerInvariant().Contains(searchTerm.ToLowerInvariant()));

            Func<IPagedList<Individual>> getIndividuals = (() => _individualService.Get(treeId, predicate, pageIndex, pageSize));

            return GetPage(getIndividuals, ind => new IndividualViewModel(ind, _factService, 0, 0, false));
        }
    }
}