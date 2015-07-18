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
    public class TreeController : BaseController
    {
        private ITreeService _treeService;

        public TreeController()
        {
            _treeService = new TreeService(Util.CreateUnitOfWork(new DnnCacheProvider()));
        }

        public HttpResponseMessage GetTrees()
        {
            return GetEntities(() => _treeService.GetTrees(), tree => new TreeViewModel(tree));
        }
    }
}