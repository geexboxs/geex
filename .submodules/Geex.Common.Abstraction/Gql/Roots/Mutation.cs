using System;

using Geex.Common.Abstraction;
using Geex.Common.Abstraction.Gql;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using MongoDB.Entities;

namespace Geex.Common.Gql.Roots
{
    public abstract class Mutation : IEmptyObject
    {
        public string _ { get; set; }
    }
}
