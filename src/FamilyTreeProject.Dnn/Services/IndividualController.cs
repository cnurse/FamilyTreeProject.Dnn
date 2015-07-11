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
            _individualService = new IndividualService(Util.CreateUnitOfWork());
        }

        public HttpResponseMessage GetIndividual(int treeId, int id, int includeAncestors = 0, int includeDescendants = 0, bool includeSpouses = false)
        {
            var settings = new IndividualServiceSettings
                                {
                                    TreeId = treeId,
                                    IncludeChildren = (includeDescendants > 0),
                                    IncludeParents = (includeAncestors > 0),
                                    IncludeSpouses = true
                                };

            return GetEntity(() => _individualService.GetIndividual( id, settings)
                                    , ind => new IndividualViewModel(ind, includeAncestors, includeDescendants, includeSpouses));
        }

        public HttpResponseMessage GetIndividuals(int treeId, int includeAncestors = 0, int includeDescendants = 0, bool includeSpouses = false)
        {
            var settings = new IndividualServiceSettings
                                {
                                    TreeId = treeId,
                                    IncludeChildren = (includeDescendants > 0),
                                    IncludeParents = (includeAncestors > 0),
                                    IncludeSpouses = true
                                };

            return GetEntities(() => _individualService.GetIndividuals(settings)
                                    , ind => new IndividualViewModel(ind, includeAncestors, includeDescendants, includeSpouses));
        }
    }
}