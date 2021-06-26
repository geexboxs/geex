using Geex.Common.Settings.Abstraction;
using MediatR;

namespace Geex.Common.Settings.Api.Aggregates.Settings.Inputs
{
    public class UpdateSettingInput : IRequest<ISetting>
    {
        public SettingDefinition Name { get; set; }
        public string? Value { get; set; }
        public string? ScopedKey { get; set; }
        public SettingScopeEnumeration Scope { get; set; }
    }
}