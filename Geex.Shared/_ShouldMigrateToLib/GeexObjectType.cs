using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HotChocolate.Types;
using MongoDB.Entities;

namespace Geex.Shared._ShouldMigrateToLib
{
    //public class GeexObjectType<T>:ObjectType<T>
    //{
    //    protected override void Configure(IObjectTypeDescriptor<T> descriptor)
    //    {
    //        //foreach (var reference in typeof(T).GetProperties(BindingFlags.Public|BindingFlags.Instance).Where(x=>x.PropertyType.Name == typeof(Many<>).Name))
    //        //{
    //        //    descriptor.Field(reference).ResolveWith()
    //        //}

    //        base.Configure(descriptor);
    //    }
    //}
}
