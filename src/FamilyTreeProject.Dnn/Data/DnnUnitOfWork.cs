//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System;
using System.Collections.Generic;
using DotNetNuke.Data;
using DotNetNuke.Data.PetaPoco;
using Naif.Core.Caching;
using Naif.Data;
using PetaPoco;

namespace FamilyTreeProject.Dnn.Data
{
    public class DnnUnitOfWork : IUnitOfWork
    {
        private readonly ICacheProvider _cache;
        private readonly Database _database;
        private readonly Dictionary<Type, IMapper> _mappers;

        private string GetTableName(string tableName)
        {
            return DataProvider.Instance().ObjectQualifier + "FTP_" + tableName;
        }

        public DnnUnitOfWork(string connectionStringName, ICacheProvider cache)
        {
            _database = new Database(connectionStringName);
            _cache = cache;
            _mappers = new Dictionary<Type, IMapper>
                            {
                                {
                                    typeof (Tree), CreateMapper<Tree>()
                                                        .TableName(GetTableName("Trees"))
                                                        .PrimaryKey("TreeID")
                                                        .Property(a => a.TreeId, "TreeID")
                                                        .Property(a => a.HomeIndividualId, "HomeIndividualID")
                                                        .Property(a => a.ImageId, "ImageID")
                                                        .Property(a => a.LastViewedIndividualId, "LastViewedIndividualID")
                                                        .Property(a => a.Name, "Name")
                                                        .Property(a => a.OwnerId, "OwnerID")
                                                        .Property(a => a.Title, "Title")
                                                        .Property(a => a.Description, "Description")
                                },
                                {
                                    typeof (Repository), CreateMapper<Repository>()
                                                            .TableName(GetTableName("Repositories"))
                                                            .PrimaryKey("ID")
                                                            .CacheKey("FTP_Repositories")
                                                            .Scope("TreeId")
                                                            .Property(a => a.Id, "ID")
                                                            .Property(a => a.TreeId, "TreeID")
                                                            .Property(a => a.Name, "Name")
                                                            .Property(a => a.Address, "Address")

                                },
                                {
                                    typeof (Source), CreateMapper<Source>()
                                                            .TableName(GetTableName("Sources"))
                                                            .PrimaryKey("ID")
                                                            .CacheKey("FTP_Sources")
                                                            .Scope("TreeId")
                                                            .Property(a => a.Id, "ID")
                                                            .Property(a => a.TreeId, "TreeID")
                                                            .Property(a => a.RepositoryId, "RepositoryID")
                                                            .Property(a => a.Author, "Author")
                                                            .Property(a => a.Publisher, "Publisher")
                                                            .Property(a => a.Title, "Title")
                                },
                                {
                                    typeof (Citation), CreateMapper<Citation>()
                                                            .TableName(GetTableName("Citations"))
                                                            .PrimaryKey("ID")
                                                            .CacheKey("FTP_Citations")
                                                            .Scope("TreeId")
                                                            .Property(a => a.Id, "ID")
                                                            .Property(a => a.TreeId, "TreeID")
                                                            .Property(a => a.SourceId, "SourceID")
                                                            .Property(a => a.OwnerId, "OwnerID")
                                                            .Property(a => a.OwnerType, "OwnerType")
                                                            .Property(a => a.Date, "Date")
                                                            .Property(a => a.Page, "Page")
                                                            .Property(a => a.Text, "Text")
                                },
                                {
                                    typeof (Note), CreateMapper<Note>()
                                                            .TableName(GetTableName("Notes"))
                                                            .PrimaryKey("ID")
                                                            .CacheKey("FTP_Notes")
                                                            .Scope("TreeId")
                                                            .Property(a => a.Id, "ID")
                                                            .Property(a => a.TreeId, "TreeID")
                                                            .Property(a => a.Text, "Text")
                                                            .Property(a => a.OwnerId, "OwnerID")
                                                            .Property(a => a.OwnerType, "OwnerType")
                                },
                                {
                                    typeof (MultimediaLink), CreateMapper<MultimediaLink>()
                                                            .TableName(GetTableName("MultimediaLinks"))
                                                            .PrimaryKey("ID")
                                                            .CacheKey("FTP_MultimediaLinks")
                                                            .Scope("TreeId")
                                                            .Property(a => a.Id, "ID")
                                                            .Property(a => a.TreeId, "TreeID")
                                                            .Property(a => a.OwnerId, "OwnerID")
                                                            .Property(a => a.OwnerType, "OwnerType")
                                                            .Property(a => a.File, "File")
                                                            .Property(a => a.Format, "Format")
                                                            .Property(a => a.Title, "Title")
                                },
                                {
                                    typeof (Individual), CreateMapper<Individual>()
                                                            .TableName(GetTableName("Individuals"))
                                                            .PrimaryKey("ID")
                                                            .CacheKey("FTP_Individuals")
                                                            .Scope("TreeId")
                                                            .Property(a => a.Id, "ID")
                                                            .Property(a => a.TreeId, "TreeID")
                                                            .Property(a => a.FirstName, "FirstName")
                                                            .Property(a => a.LastName, "LastName")
                                                            .Property(a => a.Sex, "Sex")
                                                            .Property(a => a.FatherId, "FatherID")
                                                            .Property(a => a.MotherId, "MotherID")
                                                            .Property(a => a.ImageId, "ImageID")
                                },
                                {
                                    typeof (Family), CreateMapper<Family>()
                                                            .TableName(GetTableName("Families"))
                                                            .PrimaryKey("ID")
                                                            .CacheKey("FTP_Families")
                                                            .Scope("TreeId")
                                                            .Property(a => a.Id, "ID")
                                                            .Property(a => a.TreeId, "TreeID")
                                                            .Property(a => a.HusbandId, "HusbandID")
                                                            .Property(a => a.WifeId, "WifeID")
                                },
                                {
                                    typeof(Fact), CreateMapper<Fact>()
                                                            .TableName(GetTableName("Facts"))
                                                            .PrimaryKey("ID")
                                                            .CacheKey("FTP_Facts")
                                                            .Scope("TreeId")
                                                            .Property(a => a.Id, "ID")
                                                            .Property(a => a.TreeId, "TreeID")
                                                            .Property(a => a.OwnerId, "OwnerID")
                                                            .Property(a => a.OwnerType, "OwnerType")
                                                            .Property(a => a.FactType, "FactType")
                                                            .Property(a => a.Date, "Date")
                                                            .Property(a => a.Place, "Place")
                               }
                            };

        }

        public void Dispose()
        {
        }

        public void Commit()
        {
        }

        private static FluentMapper<T> CreateMapper<T>()
        {
            return new FluentMapper<T>();
        }

        public Naif.Data.IRepository<TModel> GetRepository<TModel>() where TModel : class
        {
            var mapper = _mappers[typeof (TModel)];
            var rep = new PetaPocoRepository<TModel>(_database, mapper);
            var dnnRep = new DnnRepository<TModel>(rep, _cache);

            var fluentMapper = mapper as FluentMapper<TModel>;
            if (fluentMapper != null)
            {
                if (!String.IsNullOrEmpty(fluentMapper.TableInfo.PrimaryKey))
                {
                    dnnRep.PrimaryKey = fluentMapper.TableInfo.PrimaryKey;
                }
                if (!String.IsNullOrEmpty(fluentMapper.CacheKey))
                {
                    dnnRep.CacheKey = fluentMapper.CacheKey;
                    dnnRep.IsCacheable = true;
                }
                if (!String.IsNullOrEmpty(fluentMapper.Scope))
                {
                    dnnRep.CacheKey = dnnRep.CacheKey + "_" + fluentMapper.Scope + "_{0}";
                    dnnRep.Scope = fluentMapper.Scope;
                    dnnRep.IsScoped = true;
                }
            }


            return dnnRep;
        }
    }
}