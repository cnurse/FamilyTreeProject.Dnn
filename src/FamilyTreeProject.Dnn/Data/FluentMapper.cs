//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

// Influenced by http://sameproblemmorecode.blogspot.nl/2013/07/petapoco-as-its-meant-to-be-with.html
// https://github.com/luuksommers/PetaPoco

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using PetaPoco;

namespace FamilyTreeProject.Dnn.Data
{
    public class FluentMapper<TModel> : IMapper
    {
        public FluentMapper()
        {
            CacheKey = String.Empty;
            Mappings = new Dictionary<string, FluentColumnMap>();
            Scope = String.Empty;
            TableInfo = new TableInfo();
        }

        public string CacheKey { get; set; }

        public Dictionary<string, FluentColumnMap> Mappings { get; set; }

        public string Scope { get; set; }

        public TableInfo TableInfo { get; set; }

        public TableInfo GetTableInfo(Type pocoType)
        {
            return TableInfo;
        }

        public ColumnInfo GetColumnInfo(PropertyInfo pocoProperty)
        {
            var fluentMap = default(FluentColumnMap);
            if (Mappings.TryGetValue(pocoProperty.Name, out fluentMap))
                return fluentMap.ColumnInfo;
            return null;
        }

        public Func<object, object> GetFromDbConverter(PropertyInfo targetProperty, Type sourceType)
        {
            // ReSharper disable once RedundantAssignment
            var fluentMap = default(FluentColumnMap);
            if (Mappings.TryGetValue(targetProperty.Name, out fluentMap))
                return fluentMap.FromDbConverter;
            return null;
        }

        public Func<object, object> GetToDbConverter(PropertyInfo sourceProperty)
        {
            // ReSharper disable once RedundantAssignment
            var fluentMap = default(FluentColumnMap);
            if (Mappings.TryGetValue(sourceProperty.Name, out fluentMap))
                return fluentMap.ToDbConverter;
            return null;
        }
    }
}