//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DotNetNuke.Common;
using DotNetNuke.Services.FileSystem;
using DotNetNuke.Web.Api;
using DotNetNuke.Web.Api.Internal;
using FamilyTreeProject.Dnn.Common;
using FamilyTreeProject.Dnn.Data;
using FamilyTreeProject.Dnn.ViewModels;
using FamilyTreeProject.DomainServices;

namespace FamilyTreeProject.Dnn.Services
{
    [AllowAnonymous]
    public class TreeController : BaseController
    {
        private readonly IFamilyService _familyService;
        private readonly IIndividualService _individualService;
        private readonly IFactService _factService;
        private readonly ITreeService _treeService;

        public TreeController()
        {
            var cache = Util.CreateCacheProvider();
            var unitOfWork = Util.CreateUnitOfWork(cache);
            var serviceFactory = new FamilyTreeServiceFactory(unitOfWork, cache);
            _factService = serviceFactory.CreateFactService();
            _familyService = serviceFactory.CreateFamilyService();
            _individualService = serviceFactory.CreateIndividualService();
            _treeService = serviceFactory.CreateTreeService();
        }

        protected override string LocalResourceFile
        {
            get
            {
                return "~/DesktopModules/FTP/FamilyTreeProject/App_LocalResources/FamilyTreeProject.resx";
            }
        }

        private TreeViewModel GetTreeViewModel(Tree tree)
        {
            var treeViewModel = new TreeViewModel(tree);

            if (tree.HomeIndividualId > -1)
            {
                var homeIndividual = _individualService.Get(tree.HomeIndividualId, tree.TreeId);
                if (homeIndividual != null)
                {
                    treeViewModel.HomeIndividual = new IndividualViewModel(homeIndividual);
                }
            }
            if (tree.LastViewedIndividualId > -1)
            {
                var lastViewedIndividual = _individualService.Get(tree.LastViewedIndividualId, tree.TreeId);
                if (lastViewedIndividual != null)
                {
                    treeViewModel.LastViewedIndividual = new IndividualViewModel(lastViewedIndividual);
                }

            }
            treeViewModel.IndividualCount = _individualService.Get(tree.TreeId).Count();
            treeViewModel.FamilyCount = _familyService.Get(tree.TreeId).Count();
            treeViewModel.FactCount = _factService.Get(tree.TreeId).Count();

            if (tree.ImageId == -1)
            {
                treeViewModel.ImageUrl = "DesktopModules/FTP/FamilyTreeProject/Images/no-image-thumb.png";
            }
            else
            {
                var file = FileManager.Instance.GetFile(tree.ImageId);
                treeViewModel.ImageUrl = (file.PortalId == -1) 
                                            ? Globals.HostPath + file.RelativePath
                                            : PortalSettings.HomeDirectory + file.RelativePath;
            }

            return treeViewModel;
        }

        [HttpGet]
        public HttpResponseMessage GetTree(int treeId)
        {
            return GetEntity(() =>
                            _treeService.Get(treeId),
                            // ReSharper disable once ConvertClosureToMethodGroup
                            tree => GetTreeViewModel(tree)
                );
        }

        [HttpGet]
        public HttpResponseMessage GetTrees()
        {
            var settingsViewModel = new SettingsViewModel(ActiveModule);

            return GetEntities(() =>
                        {
                            var trees = (settingsViewModel.Owner == "user")
                                        ? _treeService.Get().Where(t => t.OwnerId == PortalSettings.UserId) 
                                        : _treeService.Get().Where(t => t.OwnerId == ActiveModule.ModuleID);
                            return trees;
                        }, 
                        tree => new TreeViewModel(tree)
                );
        }

        [DnnAuthorize]
        [HttpPost]
        public HttpResponseMessage SaveTree(TreeViewModel viewModel)
        {
            var settingsViewModel = new SettingsViewModel(ActiveModule);

            Tree tree;

            if (viewModel.TreeId == -1)
            {
                tree = new Tree
                                {
                                    TreeId = -1,
                                    Name = viewModel.Name,
                                    Title = viewModel.Title,
                                    Description = viewModel.Description,
                                    OwnerId = (settingsViewModel.Owner == "user") 
                                                ? UserInfo.UserID
                                                : ActiveModule.ModuleID
                                };
                _treeService.Add(tree);
            }
            else
            {
                tree = _treeService.Get(viewModel.TreeId);
                tree.Description = viewModel.Description;
                tree.Name = viewModel.Name;
                tree.Title = viewModel.Title;
                if (viewModel.ImageId > 0)
                {
                    tree.ImageId = viewModel.ImageId;
                }
                _treeService.Update(tree);
            }

            var response = new
                            {
                                treeId = tree.TreeId
                            };

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [DnnAuthorize]
        [HttpPost]
        [IFrameSupportedValidateAntiForgeryToken]
        public Task<HttpResponseMessage> UploadTree()
        {
            return UploadFileInternal((result, file) =>
                                        {
                                            //Parse tree
                                            var importer = new GEDCOMImporter();
                                            var settingsViewModel = new SettingsViewModel(ActiveModule);
                                            var ownerId = (settingsViewModel.Owner == "user") ? UserInfo.UserID : ActiveModule.ModuleID;
                                            result.TreeId = importer.Import(file.PhysicalPath, ownerId);
                                        });
        }
    }
}