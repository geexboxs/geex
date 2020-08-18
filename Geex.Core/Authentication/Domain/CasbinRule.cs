using System.Collections.Generic;

using Volo.Abp.Domain.Entities;

namespace Geex.Shared._ShouldMigrateToLib.Auth
{
    public class CasbinRule : Volo.Abp.Domain.Values.ValueObject, IEntity
    {
        public string PType { get; set; }
        public string V0 { get; set; }
        public string V1 { get; set; }
        public string V2 { get; set; }
        public string V3 { get; set; }
        public string V4 { get; set; }
        public string V5 { get; set; }

        public object[] GetKeys()
        {
            return new object[] { this.GetAtomicValues().JoinAsString("") };
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.PType;
            yield return this.V0;
            yield return this.V1;
            yield return this.V2;
            yield return this.V3;
            yield return this.V4;
            yield return this.V5;
        }
    }
}