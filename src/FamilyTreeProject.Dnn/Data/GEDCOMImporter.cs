//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System.Collections.Generic;
using System.Configuration;
using System.IO;
using FamilyTreeProject.Common;
using FamilyTreeProject.Data.GEDCOM;
using FamilyTreeProject.Dnn.Common;
using FamilyTreeProject.DomainServices;

namespace FamilyTreeProject.Dnn.Data
{
    public class GEDCOMImporter
    {
        private static IFamilyTreeServiceFactory _serviceFactory;
        private static Dictionary<int, int> _individualLookup;
        private static Dictionary<int, int> _repositoryLookup;
        private static Dictionary<int, int> _sourceLookup;

        public GEDCOMImporter()
        {
            var cache = Util.CreateCacheProvider();
            var unitOfWork = Util.CreateUnitOfWork(cache);
            _serviceFactory = new FamilyTreeServiceFactory(unitOfWork, cache);
        }

        public int Import(string filePath, int ownerId)
        {
            //Create GEDCOM Store
            var treeName = Path.GetFileNameWithoutExtension(filePath);
            var store = new GEDCOMStore(filePath);

            var treeService = _serviceFactory.CreateTreeService();

            var tree = new Tree() { Name = treeName, OwnerId = ownerId};
            treeService.Add(tree);

            _repositoryLookup = new Dictionary<int, int>();

            //Add Repositories
            ProcessRepositories(store.Repositories, tree.TreeId);

            _sourceLookup = new Dictionary<int, int>();

            //Add Sources
            ProcessSources(store.Sources, tree.TreeId);

            _individualLookup = new Dictionary<int, int>();

            //Add Individuals
            ProcessIndividuals(store.Individuals, tree.TreeId);

            //Add Families
            ProcessFamilies(store.Families, tree.TreeId);

            return tree.TreeId;
        }

        private static void ProcessCitations(Entity entity, IList<Citation> citations)
        {
            var citationService = _serviceFactory.CreateCitationService();

            foreach (var citation in citations)
            {
                citation.TreeId = entity.TreeId;
                citation.OwnerId = entity.Id;
                if (citation.SourceId.HasValue && citation.SourceId > 0)
                {
                    citation.SourceId = _sourceLookup[citation.SourceId.Value];
                }

                citationService.Add(citation);

                ProcessMultimedia(citation);

                ProcessNotes(citation);
            }
        }

        private static void ProcessMultimedia(Entity entity)
        {
            var multimediaService = _serviceFactory.CreateMultimediaService();

            foreach (var multimediaLink in entity.Multimedia)
            {
                multimediaLink.TreeId = entity.TreeId;
                multimediaLink.OwnerId = entity.Id;

                multimediaService.Add(multimediaLink);

                ProcessNotes(multimediaLink);
            }
        }

        private static void ProcessFacts(AncestorEntity entity)
        {
            var factService = _serviceFactory.CreateFactService();

            foreach (var fact in entity.Facts)
            {
                fact.TreeId = entity.TreeId;
                fact.OwnerId = entity.Id;
                factService.Add(fact);

                ProcessMultimedia(fact);

                ProcessNotes(fact);

                ProcessCitations(fact, fact.Citations);
            }
        }

        private static void ProcessFamilies(List<Family> families, int treeId)
        {
            var familyService = _serviceFactory.CreateFamilyService();

            foreach (var family in families)
            {
                family.TreeId = treeId;
                if (family.HusbandId.HasValue && family.HusbandId.Value > 0)
                {
                    family.HusbandId = _individualLookup[family.HusbandId.Value];
                }
                if (family.WifeId.HasValue && family.WifeId.Value > 0)
                {
                    family.WifeId = _individualLookup[family.WifeId.Value];
                }
                familyService.Add(family);

                ProcessFacts(family);

                ProcessMultimedia(family);

                ProcessNotes(family);

                ProcessCitations(family, family.Citations);
            }
        }

        private static void ProcessIndividuals(List<Individual> individuals, int treeId)
        {
            var individualService = _serviceFactory.CreateIndividualService();

            foreach (var individual in individuals)
            {
                var originalId = individual.Id;

                individual.TreeId = treeId;
                individualService.Add(individual);

                _individualLookup[originalId] = individual.Id;

                ProcessFacts(individual);

                ProcessMultimedia(individual);

                ProcessNotes(individual);

                ProcessCitations(individual, individual.Citations);
            }

            //Update Individuals with correct Link IDs
            foreach (var individual in individuals)
            {
                if (individual.FatherId.HasValue && individual.FatherId.Value > 0)
                {
                    individual.FatherId = _individualLookup[individual.FatherId.Value];
                }
                if (individual.MotherId.HasValue && individual.MotherId.Value > 0)
                {
                    individual.MotherId = _individualLookup[individual.MotherId.Value];
                }

                individualService.Update(individual);
            }
        }

        private static void ProcessNotes(Entity entity)
        {
            var noteService = _serviceFactory.CreateNoteService();

            foreach (var note in entity.Notes)
            {
                note.TreeId = entity.TreeId;
                note.OwnerId = entity.Id;
                noteService.Add(note);
            }
        }

        private static void ProcessRepositories(List<Repository> repositories, int treeId)
        {
            var repositoryService = _serviceFactory.CreateRepositoryService();

            foreach (var repository in repositories)
            {
                var originalId = repository.Id;

                repository.TreeId = treeId;
                repositoryService.Add(repository);

                _repositoryLookup[originalId] = repository.Id;

                ProcessNotes(repository);
            }
        }

        private static void ProcessSources(List<Source> sources, int treeId)
        {
            var sourceService = _serviceFactory.CreateSourceService();

            foreach (var source in sources)
            {
                var originalId = source.Id;

                source.TreeId = treeId;
                if (source.RepositoryId.HasValue && source.RepositoryId > 0)
                {
                    source.RepositoryId = _repositoryLookup[source.RepositoryId.Value];
                }

                sourceService.Add(source);

                _sourceLookup[originalId] = source.Id;

                ProcessNotes(source);
            }
        }

    }
}
