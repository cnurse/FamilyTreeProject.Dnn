//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System.Net.Http;
using FamilyTreeProject.Dnn.Common;
using FamilyTreeProject.Dnn.ViewModels;
using FamilyTreeProject.DomainServices;

namespace FamilyTreeProject.Dnn.Services
{
    public class FamilyController : BaseController
    {
        private IFamilyService _familyService;

        public FamilyController()
        {
            _familyService = new FamilyService(Util.CreateUnitOfWork(new DnnCacheProvider()));
        }

        public HttpResponseMessage GetFamilies(int treeId)
        {
            return GetEntities(() => _familyService.GetFamilies(treeId, false), ind => new FamilyViewModel(ind));
        }
    }
}