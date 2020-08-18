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
    public class ActiveRecordEntity<T> : Entity<ObjectId>, IActiveRecord<T> where T : Entity<ObjectId>
    {
        public IRepository<T, ObjectId> _repository { get; set; }
    }


    public interface IActiveRecord<T> : IEntity<ObjectId> where T : Entity<ObjectId>
    {
        public static IRepository<T, ObjectId> StaticRepository => ServiceLocator.Current.GetInstance<IRepository<T, ObjectId>>();
        public IRepository<T, ObjectId> _repository { get; set; }

        public virtual IRepository<T, ObjectId> Repository
        {
            get
            {
                if (_repository == default)
                {
                    this.AttachRepository(ServiceLocator.Current.GetInstance<IRepository<T, ObjectId>>());
                }
                return _repository;
            }
        }

        public T AttachRepository(IRepository<T, ObjectId> repositoryProvider)
        {
            _repository = repositoryProvider;
            return this as T;
        }

        public virtual Task SaveAsync(bool autoSave = false, CancellationToken cancellationToken = default)
        {
            if (this.Id == ObjectId.Empty)
            {
                return Repository.InsertAsync(this as T, autoSave, cancellationToken);
            }
            return Repository.UpdateAsync(this as T, autoSave, cancellationToken);
        }

        public virtual Task DeleteAsync(bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var task = Repository.DeleteAsync(this as T, autoSave, cancellationToken);
            this._repository = null;
            return task;
        }
    }

    public abstract class ActiveRecordAggregateRoot<T> : AggregateRoot<ObjectId>, IActiveRecord<T> where T : AggregateRoot<ObjectId>
    {
        public IRepository<T, ObjectId> _repository { get; set; }
    }
}
