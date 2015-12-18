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

        public IndividualController()
        {
            var cache = Util.CreateCacheProvider();
            var unitOfWork = Util.CreateUnitOfWork(cache);
            var serviceFactory = new FamilyTreeServiceFactory(unitOfWork, cache);
            _factService = serviceFactory.CreateFactService();
            _familyService = serviceFactory.CreateFamilyService();
            _individualService = serviceFactory.CreateIndividualService();
        }

        private FamilyViewModel GetFamilyViewModel(Family family)
        {
            var fam = family.Clone();

            var familyViewModel = new FamilyViewModel(fam);

            if (familyViewModel.HusbandId > 0)
            {
                familyViewModel.Husband = GetIndividualViewModel(_individualService.Get(familyViewModel.HusbandId, fam.TreeId));
            }
            if (familyViewModel.WifeId > 0)
            {
                familyViewModel.Wife = GetIndividualViewModel(_individualService.Get(familyViewModel.WifeId, fam.TreeId));
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
                    individualViewModel.Father = GetIndividualViewModel(_individualService.Get(individualViewModel.FatherId, ind.TreeId), includeAncestors-1, includeFamilies);
                }
                if (individualViewModel.MotherId > 0)
                {
                    individualViewModel.Mother = GetIndividualViewModel(_individualService.Get(individualViewModel.MotherId, ind.TreeId), includeAncestors - 1, includeFamilies);
                }
            }

            if (includeFamilies)
            {
                individualViewModel.Families = new List<FamilyViewModel>();

                var families = _familyService.Get(ind.TreeId,
                                fam => ind.Sex == Sex.Male ? fam.HusbandId == ind.Id : fam.WifeId == ind.Id);
                foreach (var family in families)
                {
                    individualViewModel.Families.Add(GetFamilyViewModel(family));
                }
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
        public HttpResponseMessage GetIndividual(int treeId, int id, int includeAncestors = 0, bool includeFamilies = false)
        {
            return GetEntity(() => _individualService.Get( id, treeId)
                                    // ReSharper disable once ConvertClosureToMethodGroup
                                    , ind => GetIndividualViewModel(ind, includeAncestors, includeFamilies));
        }

        [HttpGet]
        public HttpResponseMessage GetIndividuals(int treeId, string searchTerm, int pageIndex, int pageSize)
        {
            Func<Individual, bool> predicate = (ind => (String.IsNullOrEmpty(searchTerm)) || ind.Name.ToLowerInvariant().Contains(searchTerm.ToLowerInvariant()));

            Func<IPagedList<Individual>> getIndividuals = (() => _individualService.Get(treeId, predicate, pageIndex, pageSize));

            // ReSharper disable once ConvertClosureToMethodGroup
            return GetPage(getIndividuals, ind => GetIndividualViewModel(ind));
        }

        public HttpResponseMessage SaveIndividual(IndividualViewModel viewModel)
        {
            Individual individual;

            if (viewModel.Id == -1)
            {
                individual = new Individual
                {
                    Id = -1,
                    TreeId = -1,
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName
                };
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
                individual = individual.Id
            };

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}