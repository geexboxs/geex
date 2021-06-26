using System.Linq;
using Geex.Common.Settings.Abstraction;
using MediatR;

namespace Geex.Common.Settings.Api.Aggregates.Settings.Inputs
{
    public class GetSettingsInput : IRequest<IQueryable<ISetting>>
    {
        public SettingScopeEnumeration Scope { get; set; }
    }
}