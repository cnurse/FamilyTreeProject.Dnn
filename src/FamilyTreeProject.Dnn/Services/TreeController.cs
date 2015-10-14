//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.UI.WebControls;
using DotNetNuke.Collections;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Host;
using DotNetNuke.Entities.Icons;
using DotNetNuke.Entities.Users;
using DotNetNuke.Instrumentation;
using DotNetNuke.Services.FileSystem;
using DotNetNuke.Web.Api;
using DotNetNuke.Web.Api.Internal;
using DotNetNuke.Web.InternalServices;
using FamilyTreeProject.Dnn.Common;
using FamilyTreeProject.Dnn.Data;
using FamilyTreeProject.Dnn.ViewModels;
using FamilyTreeProject.DomainServices;

namespace FamilyTreeProject.Dnn.Services
{
    [AllowAnonymous]
    public class TreeController : BaseController
    {
        private static readonly ILog Logger = LoggerSource.Instance.GetLogger(typeof(TreeController));

        private readonly IFamilyService _familyService;
        private readonly IIndividualService _individualService;
        private readonly ITreeService _treeService;

        public TreeController()
        {
            var cache = Util.CreateCacheProvider();
            var unitOfWork = Util.CreateUnitOfWork(cache);
            var serviceFactory = new FamilyTreeServiceFactory(unitOfWork, cache);
            _treeService = serviceFactory.CreateTreeService();
            _familyService = serviceFactory.CreateFamilyService();
            _individualService = serviceFactory.CreateIndividualService();
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
                treeViewModel.HomeIndividual = new IndividualViewModel(_individualService.Get(tree.HomeIndividualId, tree.TreeId));
            }
            if (tree.LastViewedIndividualId > -1)
            {
                treeViewModel.LastViewedIndividual = new IndividualViewModel(_individualService.Get(tree.LastViewedIndividualId, tree.TreeId));
            }
            treeViewModel.IndividualCount = _individualService.Get(tree.TreeId).Count();
            treeViewModel.FamilyCount = _familyService.Get(tree.TreeId).Count();

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

        [DnnAuthorize]
        [HttpPost]
        public HttpResponseMessage SaveTree(TreeViewModel viewModel)
        {
            Tree tree;

            if (viewModel.TreeId == -1)
            {
                tree = new Tree
                                {
                                    TreeId = -1,
                                    Name = viewModel.Name,
                                    Title = viewModel.Title,
                                    Description = viewModel.Description,
                                    OwnerId = PortalSettings.UserId
                                };
                _treeService.Add(tree);
            }
            else
            {
                tree = _treeService.Get(viewModel.TreeId);
                tree.Description = viewModel.Description;
                tree.Name = viewModel.Name;
                tree.Title = viewModel.Title;
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
            var request = Request;
            FileUploadViewModel result = null;

            if (!request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = new MultipartMemoryStreamProvider();

            // local references for use in closure
            var currentSynchronizationContext = SynchronizationContext.Current;
            var userInfo = UserInfo;
            var overwrite = false;
            var task = request.Content.ReadAsMultipartAsync(provider)
                .ContinueWith(o =>
                {
                    var fileName = string.Empty;
                    Stream stream = null;

                    foreach (var item in provider.Contents)
                    {
                        var name = item.Headers.ContentDisposition.Name;
                        switch (name.ToLowerInvariant())
                        {
                            case "\"overwrite\"":
                                bool.TryParse(item.ReadAsStringAsync().Result, out overwrite);
                                break;
                            case "\"postfile\"":
                                fileName = item.Headers.ContentDisposition.FileName.Replace("\"", "");
                                if (fileName.IndexOf("\\", StringComparison.Ordinal) != -1)
                                {
                                    fileName = Path.GetFileName(fileName);
                                }
                                if (Regex.Match(fileName, "[\\\\/]\\.\\.[\\\\/]").Success == false)
                                {
                                    stream = item.ReadAsStreamAsync().Result;
                                }
                                break;
                        }
                    }

                    if (!string.IsNullOrEmpty(fileName) && stream != null)
                    {
                        // The SynchronizationContext keeps the main thread context. Send method is synchronous
                        currentSynchronizationContext.Send(
                            delegate
                            {
                                result = UploadTree(stream, userInfo, fileName, overwrite);
                            },
                            null
                        );
                    }

                    var mediaTypeFormatter = new JsonMediaTypeFormatter();
                    mediaTypeFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/plain"));

                    return Request.CreateResponse(HttpStatusCode.OK, result, mediaTypeFormatter, "text/plain");
                });

            return task;
        }

        private FileUploadViewModel UploadTree(Stream stream, UserInfo user, string fileName, bool overwrite)
        {
            var result = new FileUploadViewModel();
            try
            {
                var extension = Path.GetExtension(fileName).ValueOrEmpty().Replace(".", "");
                result.FileIconUrl = IconController.GetFileIconUrl(extension);

                if (string.IsNullOrEmpty(extension) || !Host.AllowedExtensionWhitelist.IsAllowedExtension(extension))
                {
                    result.Message = LocalizeString("ExtensionNotAllowed");
                    return result;
                }

                var folderManager = FolderManager.Instance;

                // Check if this is a User Folder                
                var folderInfo = folderManager.GetUserFolder(user);

                IFileInfo file;

                if (!overwrite && FileManager.Instance.FileExists(folderInfo, fileName, true))
                {
                    result.Message = LocalizeString("AlreadyExists");
                    result.AlreadyExists = true;
                    file = FileManager.Instance.GetFile(folderInfo, fileName, true);
                    result.FileId = file.FileId;
                }
                else
                {
                    file = FileManager.Instance.AddFile(folderInfo, fileName, stream, true, false, "text/plain", user.UserID);
                    result.FileId = file.FileId;

                    //Parse tree
                    var importer = new GEDCOMImporter();
                    result.TreeId = importer.Import(file.PhysicalPath, user.UserID);
                }

                var path = FileManager.Instance.GetUrl(file);
                result.Orientation = Orientation.Vertical;

                result.Path = result.FileId > 0 ? path : string.Empty;
                result.FileName = fileName;

                return result;
            }
            catch (Exception exe)
            {
                Logger.Error(exe);
                result.Message = exe.Message;
                return result;
            }
        }

    }
}