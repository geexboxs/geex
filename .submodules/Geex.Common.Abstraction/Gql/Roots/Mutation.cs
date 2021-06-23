using System;
using CommonServiceLocator;
using MediatR;

namespace Geex.Common.Gql.Roots
{
    public abstract class Mutation
    {
        protected IMediator Mediator => ServiceLocator.Current.GetInstance<IMediator>();
        public string placeHolder { get; set; }
    }
}
