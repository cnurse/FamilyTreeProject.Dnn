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
    public class FamilyController : BaseController
    {
        private IFamilyService _familyService;

        public FamilyController()
        {
            var cache = Util.CreateCacheProvider();
            var unitOfWork = Util.CreateUnitOfWork(cache);
            var serviceFactory = new FamilyTreeServiceFactory(unitOfWork, cache);
            _familyService = serviceFactory.CreateFamilyService();
        }


        public HttpResponseMessage GetFamilies(int treeId)
        {
            return GetEntities(() => _familyService.Get(treeId), fam => new FamilyViewModel(fam));
        }
    }
}