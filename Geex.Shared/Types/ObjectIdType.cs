using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Language;
using HotChocolate.Properties;
using HotChocolate.Types;
using MongoDB.Bson;

namespace Geex.Shared.Types
{
    public class ObjectIdType : ScalarType
    {
        public ObjectIdType() : base("ObjectId")
        {
        }

        public override bool IsInstanceOfType(IValueNode literal)
        {
            if (literal == null)
            {
                throw new ArgumentNullException(nameof(literal));
            }

            return literal is StringValueNode
                || literal is IntValueNode
                || literal is NullValueNode;
        }

        public override object ParseLiteral(IValueNode literal)
        {
            if (literal == null)
            {
                throw new ArgumentNullException(nameof(literal));
            }

            if (literal is StringValueNode stringLiteral)
            {
                return ObjectId.Parse(stringLiteral.Value);
            }

            throw new ScalarSerializationException("");
        }

        public override IValueNode ParseValue(object value)
        {
            if (value is ObjectId s)
            {
                return new StringValueNode(s.ToString());
            }

            throw new ScalarSerializationException("");
        }

        public override object Serialize(object value)
        {
            if (value is ObjectId s)
            {
                return s;
            }

            throw new ScalarSerializationException("");
        }

        public override bool TryDeserialize(object serialized, out object value)
        {
            if (serialized is string str)
            {
                value = ObjectId.Parse(str);
                return true;
            }

            if (serialized is ObjectId)
            {
                value = serialized;
                return true;
            }

            value = null;
            return false;
        }

        public override Type ClrType { get; } = typeof(ObjectId);
    }
}
