using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Humanizer;
using MongoDB.Driver;
using Repository.Mongo;

namespace Geex.Shared._ShouldMigrateToLib
{
    public class MongoCollectionRegistrationSource : IRegistrationSource
    {
        public bool IsAdapterForIndividualComponents
        {
            get { return true; }
        }

        public IEnumerable<IComponentRegistration> RegistrationsFor(
            Service service,
            Func<Service, IEnumerable<IComponentRegistration>> registrationAccessor)
        {
            var swt = service as IServiceWithType;
            if (swt == null || !swt.ServiceType.IsGenericType)
                yield break;

            var def = swt.ServiceType.GetGenericTypeDefinition();
            if (def != typeof(Repository<>))
                yield break;

            // if you have one `IDBContext` registeration you don't need the
            // foreach over the registrationAccessor(dbContextServices)

            yield return RegistrationBuilder.ForDelegate((c, p) =>
            {
                var repoType = typeof(Repository<>).MakeGenericType(swt.ServiceType.GetGenericArguments());
                var mongoDatabase = c.Resolve<IMongoDatabase>();
                var ctor = repoType.GetConstructor(new[] { typeof(IMongoDatabase), typeof(string) });
                Func<object> paramProvider = default;
                var collectionName = p.FirstOrDefault()?.CanSupplyValue(ctor.GetParameters()[1], c, out paramProvider) != default ? paramProvider.Invoke() : swt.ServiceType.GetGenericArguments()[0].Name.Pluralize();
                return ctor.Invoke(null, new object[] { mongoDatabase, collectionName });
            })
                    .As(service)
                    .CreateRegistration();
        }
    }
}
