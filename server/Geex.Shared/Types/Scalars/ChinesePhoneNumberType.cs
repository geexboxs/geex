﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using HotChocolate;
using HotChocolate.Language;
using HotChocolate.Types;

namespace Geex.Shared.Types.Scalars
{
    public class ChinesePhoneNumberType : RegexType
    {
        private const string _validationPattern = "^[1]([3-9])[0-9]{9}$";

        public ChinesePhoneNumberType()
          : this((NameString)"ChinesePhoneNumberType", @"^\[1\]\(\[3-9\]\)[0-9]{9}$")
        {
        }

        public ChinesePhoneNumberType(NameString name, string? description = null, BindingBehavior bind = BindingBehavior.Explicit)
          : base(name, _validationPattern, description, RegexOptions.IgnoreCase | RegexOptions.Compiled, bind)
        {
        }

        protected override SerializationException CreateParseLiteralError(
          IValueNode valueSyntax)
        {
            return new SerializationException("invalid phone number", this);
        }

        protected override SerializationException CreateParseValueError(
          object runtimeValue)
        {
            return new SerializationException("invalid phone number", this);
        }
    }
}
