//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using HotChocolate.Types;

//namespace Geex.Core.SystemSettings.GqlSchemas.Types
//{
//    public class SettingGqlType:ObjectType<Setting>
//    {
//        protected override void Configure(IObjectTypeDescriptor<Setting> descriptor)
//        {
//            descriptor.BindFieldsImplicitly();
//            descriptor.Field(x=>x.Scope);
//            base.Configure(descriptor);
//        }
//    }
//}
