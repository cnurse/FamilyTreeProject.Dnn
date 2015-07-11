//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System;
using System.Collections.Generic;
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

        public DnnUnitOfWork(string connectionStringName, ICacheProvider cache)
        {
            _database = new Database(connectionStringName);
            _cache = cache;
            _mappers = new Dictionary<Type, IMapper>
                            {
                                {
                                    typeof (Tree), CreateMapper<Tree>()
                                                        .TableName("Trees")
                                                        .PrimaryKey("TreeId")
                                                        .Property(a => a.TreeId, "TreeId")
                                                        .Property(a => a.Name, "Name")
                                                        .Property(a => a.OwnerId, "OwnerID")
                                },
                                {
                                    typeof (Individual), CreateMapper<Individual>()
                                                            .TableName("Individuals")
                                                            .PrimaryKey("ID")
                                                            .CacheKey("FTP_Individuals")
                                                            .Scope("TreeID")
                                                            .Property(a => a.Id, "ID")
                                                            .Property(a => a.TreeId, "TreeID")
                                                            .Property(a => a.FirstName, "FirstName")
                                                            .Property(a => a.LastName, "LastName")
                                                            .Property(a => a.Sex, "Sex")
                                                            .Property(a => a.FatherId, "FatherID")
                                                            .Property(a => a.MotherId, "MotherID")

                                },
                                {
                                    typeof (Family), CreateMapper<Family>()
                                                            .TableName("Families")
                                                            .PrimaryKey("ID")
                                                            .Property(a => a.TreeId, "TreeID")
                                                            .Property(a => a.HusbandId, "HusbandID")
                                                            .Property(a => a.WifeId, "WifeID")
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


        public IRepository<TModel> GetRepository<TModel>() where TModel : class
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
                    dnnRep.Scope = fluentMapper.Scope;
                    dnnRep.IsScoped = true;
                }
            }


            return dnnRep;
        }
    }
}