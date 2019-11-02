using System;
using Castle.Core.Internal;
using HotChocolate.Configuration;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using HotChocolate.Types.Descriptors.Definitions;

namespace Geex.Shared.Roots.RootTypes
{
    public class MutationType : ObjectType<Mutation>
    {
        protected override void OnCompleteType(ICompletionContext context, ObjectTypeDefinition definition)
        {
            if (definition.Fields.IsNullOrEmpty())
            {
                definition.Fields.Add(new ObjectFieldDefinition()
                {
                    Name = "__placeHolder",
                    Type = new ClrTypeReference(typeof(string), TypeContext.Output),
                    DeprecationReason = "This field is a placeholder in case of empty type.",
                    Resolver = (resolverContext => throw new Exception("This field is a placeholder in case of empty type."))
                });
            }
            base.OnCompleteType(context, definition);
        }

    }
}
