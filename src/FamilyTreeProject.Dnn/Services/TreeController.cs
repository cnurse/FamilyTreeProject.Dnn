//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using DotNetNuke.Collections;
using FamilyTreeProject.Dnn.Common;
using FamilyTreeProject.Dnn.ViewModels;
using FamilyTreeProject.DomainServices;

namespace FamilyTreeProject.Dnn.Services
{
    [AllowAnonymous]
    public class TreeController : BaseController
    {
        private readonly ITreeService _treeService;

        public TreeController()
        {
            var cache = Util.CreateCacheProvider();
            var unitOfWork = Util.CreateUnitOfWork(cache);
            var serviceFactory = new FamilyTreeServiceFactory(unitOfWork, cache);
            _treeService = serviceFactory.CreateTreeService();
        }

        public HttpResponseMessage GetTrees()
        {
            return GetEntities(() =>
                        {
                            var owner = ActiveModule.ModuleSettings.GetValueOrDefault(SettingsController.OwnerKey, "user");

                            var trees = (owner == "user")
                                        ? _treeService.Get().Where(t => t.OwnerId == PortalSettings.UserId) 
                                        : _treeService.Get().Where(t => t.OwnerId == ActiveModule.ModuleID);
                            return trees;
                        }, 
                        tree => new TreeViewModel(tree)
                );
        }
    }
}