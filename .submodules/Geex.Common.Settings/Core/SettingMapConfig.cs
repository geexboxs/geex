using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Geex.Common.Abstraction;
using Geex.Common.Abstraction.Bson;
using Geex.Common.Settings.Abstraction;

using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Geex.Common.Settings.Core
{
    public class SettingMapConfig : EntityMapConfig<Setting>
    {
        public override void Map(BsonClassMap<Setting> map)
        {
            map.AutoMap();
            map.MapMember(x => x.Name).SetSerializer(new EnumerationSerializer<SettingDefinition, string>());
            map.MapMember(x => x.Scope).SetSerializer(new EnumerationSerializer<SettingScopeEnumeration, string>());
        }
    }
}
