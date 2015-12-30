//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DotNetNuke.Common;
using DotNetNuke.Services.FileSystem;
using FamilyTreeProject.Common;
using FamilyTreeProject.Dnn.Common;
using FamilyTreeProject.Dnn.ViewModels;
using FamilyTreeProject.DomainServices;
using Naif.Core.Collections;

namespace FamilyTreeProject.Dnn.Services
{
    [AllowAnonymous]
    public class IndividualController : BaseController
    {
        private readonly IFactService _factService;
        private readonly IFamilyService _familyService;
        private readonly IIndividualService _individualService;
        private readonly ITreeService _treeService;

        public IndividualController()
        {
            var cache = Util.CreateCacheProvider();
            var unitOfWork = Util.CreateUnitOfWork(cache);
            var serviceFactory = new FamilyTreeServiceFactory(unitOfWork, cache);
            _factService = serviceFactory.CreateFactService();
            _familyService = serviceFactory.CreateFamilyService();
            _individualService = serviceFactory.CreateIndividualService();
            _treeService = serviceFactory.CreateTreeService();
        }

        [HttpPost]
        public HttpResponseMessage DeleteIndividual(Individual individual)
        {
            _individualService.Delete(individual);

            var response = new
            {
                id = individual.Id
            };

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        private FamilyViewModel GetFamilyViewModel(Family family, Sex sex)
        {
            var fam = family.Clone();

            var familyViewModel = new FamilyViewModel(fam);

            var spouseId = (sex == Sex.Male) ? fam.WifeId.GetValueOrDefault(0) : fam.HusbandId.GetValueOrDefault(0);

            if (spouseId > 0)
            {
                familyViewModel.Spouse = GetIndividualViewModel(_individualService.Get(spouseId, fam.TreeId));
            }

            var children = _individualService.Get(fam.TreeId,
                (ind) => ind.FatherId == fam.HusbandId && ind.MotherId == fam.WifeId);

            foreach (var child in children)
            {
                familyViewModel.Children.Add(GetIndividualViewModel(child));
            }

            return familyViewModel;
        }

        private IndividualViewModel GetIndividualViewModel(Individual individual, int includeAncestors = 0, bool includeFamilies = false)
        {
            var ind = individual.Clone();
            ind.Facts = _factService.Get(ind.TreeId, f => f.OwnerId == ind.Id && f.OwnerType == EntityType.Individual).ToList();

            var individualViewModel = new IndividualViewModel(ind);

            if (includeAncestors > 0)
            {
                if (individualViewModel.FatherId > 0)
                {
                    individualViewModel.Father = GetIndividualViewModel(_individualService.Get(individualViewModel.FatherId, ind.TreeId), includeAncestors-1);
                }
                if (individualViewModel.MotherId > 0)
                {
                    individualViewModel.Mother = GetIndividualViewModel(_individualService.Get(individualViewModel.MotherId, ind.TreeId), includeAncestors - 1);
                }
            }

            if (includeFamilies)
            {
                individualViewModel.Families = new List<FamilyViewModel>();

                var families = _familyService.Get(ind.TreeId,
                                fam => ind.Sex == Sex.Male ? fam.HusbandId == ind.Id : fam.WifeId == ind.Id);
                foreach (var family in families)
                {
                    individualViewModel.Families.Add(GetFamilyViewModel(family, ind.Sex));
                }
            }

            individualViewModel.Facts = new List<FactViewModel>();
            foreach (var fact in ind.Facts)
            {
                individualViewModel.Facts.Add(new FactViewModel(fact));
            }

            if (ind.ImageId == -1)
            {
                individualViewModel.ImageUrl = (ind.Sex == Sex.Female)
                                                    ? "DesktopModules/FTP/FamilyTreeProject/Images/female.png" 
                                                    : "DesktopModules/FTP/FamilyTreeProject/Images/male.png";
            }
            else
            {
                var file = FileManager.Instance.GetFile(ind.ImageId);
                individualViewModel.ImageUrl = (file.PortalId == -1)
                                            ? Globals.HostPath + file.RelativePath
                                            : PortalSettings.HomeDirectory + file.RelativePath;
            }

            return individualViewModel;
        }

        [HttpGet]
        public HttpResponseMessage GetIndividual(int treeId, int id, int includeAncestors = 0, bool includeFamilies = false, bool updateTree = false)
        {
            var response = GetEntity(() => _individualService.Get( id, treeId)
                                    // ReSharper disable once ConvertClosureToMethodGroup
                                    , ind => GetIndividualViewModel(ind, includeAncestors, includeFamilies));

            if (updateTree)
            {
                var tree = _treeService.Get(treeId);
                tree.LastViewedIndividualId = id;
                _treeService.Update(tree);
            }

            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetIndividuals(int treeId, string searchTerm, int pageIndex, int pageSize)
        {
            Func<Individual, bool> predicate = (ind => (String.IsNullOrEmpty(searchTerm)) || ind.Name.ToLowerInvariant().Contains(searchTerm.ToLowerInvariant()));

            Func<IPagedList<Individual>> getIndividuals = (() => _individualService.Get(treeId, predicate, pageIndex, pageSize));

            // ReSharper disable once ConvertClosureToMethodGroup
            return GetPage(getIndividuals, ind => GetIndividualViewModel(ind));
        }

        [HttpPost]
        public HttpResponseMessage SaveIndividual(IndividualViewModel viewModel)
        {
            Individual individual;

            if (viewModel.Id == -1)
            {
                individual = new Individual
                {
                    Id = -1,
                    TreeId = viewModel.TreeId,
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName
                };
                switch (viewModel.Sex)
                {
                    case "Male":
                        individual.Sex = Sex.Male;
                        break;
                    case "Female":
                        individual.Sex = Sex.Female;
                        break;
                    default:
                        individual.Sex = Sex.Unknown;
                        break;
                }
                _individualService.Add(individual);
            }
            else
            {
                individual = _individualService.Get(viewModel.Id, viewModel.TreeId);
                individual.FirstName = viewModel.FirstName;
                individual.LastName = viewModel.LastName;
                if (viewModel.ImageId > 0)
                {
                    individual.ImageId = viewModel.ImageId;
                }
                _individualService.Update(individual);
            }

            var response = new
            {
                id = individual.Id
            };

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}