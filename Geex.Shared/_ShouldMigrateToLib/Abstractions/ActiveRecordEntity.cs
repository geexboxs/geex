using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using MongoDB.Bson;

using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Geex.Shared._ShouldMigrateToLib.Abstractions
{
    public abstract class ActiveRecordEntity<T> : Entity<ObjectId> where T : Entity<ObjectId>
    {
        private Func<IRepository<T>> _repositoryProvider;

        public IRepository<T> Repository => _repositoryProvider.Invoke();

        public T AttachRepository(Func<IRepository<T>> repositoryProvider)
        {
            _repositoryProvider = repositoryProvider;
            return this as T;
        }

        public virtual Task SaveAsync(bool autoSave = false, CancellationToken cancellationToken = default)
        {
            return Repository.UpdateAsync(this as T, autoSave, cancellationToken);
        }

        public virtual Task DeleteAsync(bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var task = Repository.DeleteAsync(this as T, autoSave, cancellationToken);
            this._repositoryProvider = null;
            return task;
        }
    }

    public abstract class ActiveRecordAggregateRoot<T> : AggregateRoot<ObjectId> where T : AggregateRoot<ObjectId>
    {
        private Func<IRepository<T>> _repositoryProvider;

        public IRepository<T> Repository => _repositoryProvider.Invoke();

        public T AttachRepository(Func<IRepository<T>> repositoryProvider)
        {
            _repositoryProvider = repositoryProvider;
            return this as T;
        }

        public virtual Task SaveAsync(bool autoSave = false, CancellationToken cancellationToken = default)
        {
            return Repository.UpdateAsync(this as T, autoSave, cancellationToken);
        }

        public virtual Task DeleteAsync(bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var task = Repository.DeleteAsync(this as T, autoSave, cancellationToken);
            this._repositoryProvider = null;
            return task;
        }
    }
}
