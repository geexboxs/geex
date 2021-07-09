using System;

using Geex.Common.Abstraction;
using Geex.Common.Abstraction.Gql;

using HotChocolate.Types;

using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Geex.Common.Gql.Roots
{
    public abstract class Query : IEmptyObject
    {
        protected IMediator Mediator => ServiceLocator.Current.GetService<IMediator>();
        public string _ { get; set; }
    }
}
