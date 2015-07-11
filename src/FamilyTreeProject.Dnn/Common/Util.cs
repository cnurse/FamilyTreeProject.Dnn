//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using FamilyTreeProject.Dnn.Data;
using Naif.Data;

namespace FamilyTreeProject.Dnn.Common
{
    public static class Util
    {
        public static IUnitOfWork CreateUnitOfWork()
        {
            var connectionStringName = "FamilyTreeProject";

            return new DnnUnitOfWork(connectionStringName, new DnnCacheProvider());
        }
    }
}