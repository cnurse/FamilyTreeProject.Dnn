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
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Host;
using DotNetNuke.Entities.Icons;
using DotNetNuke.Instrumentation;
using DotNetNuke.Services.FileSystem;
using DotNetNuke.Services.Localization;
using DotNetNuke.Web.Api;
using FamilyTreeProject.Dnn.ViewModels;
using Naif.Core.Collections;

namespace FamilyTreeProject.Dnn.Services
{
    public class BaseController : DnnApiController
    {
        private static readonly ILog Logger = LoggerSource.Instance.GetLogger(typeof(BaseController));

        protected virtual string LocalResourceFile
        {
            get { return "~/DesktopModules/FTP/FamilyTreeProject/App_LocalResources/FamilyTreeProject.resx"; }
        }

        public HttpResponseMessage GetEntity<TEntity, TViewModel>(Func<TEntity> getEntity, Func<TEntity, TViewModel> createViewModel)
        {
            var entity = getEntity();
            var viewModel = createViewModel(entity);

            var response = new
                            {
                                results = viewModel,
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
                                results = viewModels,
                                total = entityList.Count()
                            };

            return Request.CreateResponse(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="getEntities"></param>
        /// <param name="getViewModel"></param>
        /// <returns></returns>
        protected HttpResponseMessage GetPage<TEntity, TViewModel>(Func<IPagedList<TEntity>> getEntities, Func<TEntity, TViewModel> getViewModel)
        {
            var entityList = getEntities();
            var viewModels = entityList
                // ReSharper disable once ConvertClosureToMethodGroup
                                .Select(entity => getViewModel(entity))
                                .ToList();

            var response = new
                            {
                                results = viewModels,
                                total = entityList.TotalCount
                            };

            return Request.CreateResponse(response);
        }

        protected string LocalizeString(string key)
        {
            return Localization.GetString(key, LocalResourceFile);
        }

        protected Task<HttpResponseMessage> UploadFileInternal(Action<FileUploadViewModel, IFileInfo> onUploadSuccess)
        {
            FileUploadViewModel result = null;

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = new MultipartMemoryStreamProvider();

            // local references for use in closure
            var currentSynchronizationContext = SynchronizationContext.Current;
            var overwrite = false;
            var task = Request.Content.ReadAsMultipartAsync(provider)
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
                                result = UploadFile(stream, fileName, overwrite, onUploadSuccess);
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

        private FileUploadViewModel UploadFile(Stream stream, string fileName, bool overwrite, Action<FileUploadViewModel, IFileInfo> onUploadSuccess)
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
                var fileManager = FileManager.Instance;

                // Get the user's folder                
                var userFolder = folderManager.GetUserFolder(UserInfo);

                string folderPath;
                switch (extension)
                {
                    case "ged":
                        folderPath = userFolder.FolderPath + "GEDCOM/";
                        break;
                    case "gif":
                    case "jpg":
                    case "jpeg":
                    case "png":
                        folderPath = userFolder.FolderPath + "Images/";
                        break;
                    default:
                        folderPath = userFolder.FolderPath + "Documents/";
                        break;
                }

                var folder = folderManager.GetFolders(userFolder).SingleOrDefault(f => f.FolderPath == folderPath) ??
                             folderManager.AddFolder(userFolder.PortalID, folderPath);


                IFileInfo file;

                if (!overwrite && fileManager.FileExists(folder, fileName, true))
                {
                    result.Message = LocalizeString("AlreadyExists");
                    result.AlreadyExists = true;
                    file = fileManager.GetFile(folder, fileName, true);
                    result.FileId = file.FileId;
                }
                else
                {
                    file = fileManager.AddFile(folder, fileName, stream, true, false, "text/plain", UserInfo.UserID);
                    result.FileId = file.FileId;

                    onUploadSuccess(result, file);
                }

                var path = fileManager.GetUrl(file);
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