using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Autofac.Core.Lifetime;

using CommonServiceLocator;

using HotChocolate;

using MongoDB.Bson;

using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Geex.Shared._ShouldMigrateToLib.Abstractions
{
    public abstract class ActiveRecordEntity<T> : Entity, IActiveRecord<T> where T : Entity
    {
        public virtual IRepository<T> Repository => IActiveRecord<T>.StaticRepository;
        public override object[] GetKeys()
        {
            return new object[] { Id };
        }

        public ObjectId Id { get; set; }
    }


    public interface IActiveRecord<T> : IEntity where T : Entity
    {
        public static IRepository<T> StaticRepository => ServiceLocator.Current.GetInstance<IRepository<T>>();

        public virtual IRepository<T> Repository => StaticRepository;

        public virtual Task SaveAsync(bool autoSave = false, CancellationToken cancellationToken = default)
        {
            if (this.GetKeys().Length == 1 && this.GetKeys()[0] is ObjectId objectId && objectId == ObjectId.Empty)
            {
                return Repository.InsertAsync(this as T, autoSave, cancellationToken);
            }
            if (this.GetKeys().Any(x => x == x.GetType().Default()))
            {
                return Repository.InsertAsync(this as T, autoSave, cancellationToken);
            }
            return Repository.UpdateAsync(this as T, autoSave, cancellationToken);
        }


        public virtual Task DeleteAsync(bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var task = Repository.DeleteAsync(this as T, autoSave, cancellationToken);
            return task;
        }
    }

    public abstract class ActiveRecordAggregateRoot<T> : AggregateRoot, IActiveRecord<T> where T : AggregateRoot
    {
        public virtual IRepository<T> Repository => IActiveRecord<T>.StaticRepository;
        public override object[] GetKeys()
        {
            return new object[] { Id };
        }

        public ObjectId Id { get; set; }
    }
}
