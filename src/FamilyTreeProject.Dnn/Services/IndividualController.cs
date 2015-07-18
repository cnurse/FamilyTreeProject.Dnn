//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System.Net.Http;
using System.Web.Http;
using FamilyTreeProject.Dnn.Common;
using FamilyTreeProject.Dnn.ViewModels;
using FamilyTreeProject.DomainServices;

namespace FamilyTreeProject.Dnn.Services
{
    [AllowAnonymous]
    public class IndividualController : BaseController
    {
        private readonly IIndividualService _individualService;

        public IndividualController()
        {
            var cache = Util.CreateCacheProvider();
            var unitOfWork = Util.CreateUnitOfWork(cache);
            _individualService = new IndividualService(unitOfWork, cache);
        }

        public HttpResponseMessage GetIndividual(int treeId, int id, int includeAncestors = 0, int includeDescendants = 0, bool includeSpouses = false)
        {
            return GetEntity(() => _individualService.GetIndividual( id, treeId)
                                    , ind => new IndividualViewModel(ind, includeAncestors, includeDescendants, includeSpouses));
        }

        public HttpResponseMessage GetIndividuals(int treeId, int includeAncestors = 0, int includeDescendants = 0, bool includeSpouses = false)
        {

            return GetEntities(() => _individualService.GetIndividuals(treeId)
                                    , ind => new IndividualViewModel(ind, includeAncestors, includeDescendants, includeSpouses));
        }
    }
}