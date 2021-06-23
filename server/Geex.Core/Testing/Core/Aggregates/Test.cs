using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Geex.Common.Abstractions;
using Geex.Core.Testing.Api.Aggregates;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Geex.Core.Testing.Core.Aggregates
{
    public class Test : Entity, ITest
    {
        public string Name { get; set; }
        public string Data { get; set; }
    }
}
