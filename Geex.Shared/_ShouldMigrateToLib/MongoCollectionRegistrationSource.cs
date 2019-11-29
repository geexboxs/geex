using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Humanizer;
using MongoDB.Driver;

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
            if (def != typeof(IMongoCollection<>))
                yield break;

            // if you have one `IDBContext` registeration you don't need the
            // foreach over the registrationAccessor(dbContextServices)

            yield return RegistrationBuilder.ForDelegate((c, p) =>
            {
                var mongoDatabase = c.Resolve<IMongoDatabase>();
                var m = mongoDatabase.GetType().GetMethod(nameof(IMongoDatabase.GetCollection), new Type[] { typeof(string), typeof(MongoCollectionSettings) });
                var method =
                    m.MakeGenericMethod(swt.ServiceType.GetGenericArguments());
                return method.Invoke(mongoDatabase, new object[] { swt.ServiceType.GetGenericArguments()[0].Name.Pluralize(), c.ResolveOptional<MongoCollectionSettings>() });
            })
                    .As(service)
                    .CreateRegistration();
        }
    }
}
