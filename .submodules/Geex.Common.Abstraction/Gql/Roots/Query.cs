using System;

using CommonServiceLocator;

using Geex.Common.Abstraction.Gql;

using HotChocolate.Types;

using MediatR;

namespace Geex.Common.Gql.Roots
{
    public abstract class Query : IEmptyObject
    {
        protected IMediator Mediator => ServiceLocator.Current.GetInstance<IMediator>();
        public string _ { get; set; }
    }
}
