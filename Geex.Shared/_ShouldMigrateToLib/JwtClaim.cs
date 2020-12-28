using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Text;

using Geex.Shared._ShouldMigrateToLib.Abstractions;

using JetBrains.Annotations;

namespace Geex.Shared._ShouldMigrateToLib
{
    public class JwtClaim : Claim
    {
        public JwtClaim(BinaryReader reader) : base(reader)
        {
        }

        public JwtClaim(BinaryReader reader, ClaimsIdentity subject) : base(reader, subject)
        {
        }

        protected JwtClaim(Claim other) : base(other)
        {
        }

        protected JwtClaim(Claim other, ClaimsIdentity subject) : base(other, subject)
        {
        }

        public JwtClaim(JwtClaimType type, string value) : base(type, value)
        {
        }

        public JwtClaim(JwtClaimType type, string value, string valueType) : base(type, value, valueType)
        {
        }

        public JwtClaim(JwtClaimType type, string value, string valueType, string issuer) : base(type, value, valueType, issuer)
        {
        }

        public JwtClaim(JwtClaimType type, string value, string valueType, string issuer, string originalIssuer) : base(type, value, valueType, issuer, originalIssuer)
        {
        }

        public JwtClaim(JwtClaimType type, string value, string valueType, string issuer, string originalIssuer, ClaimsIdentity subject) : base(type, value, valueType, issuer, originalIssuer, subject)
        {
        }
    }

    public class JwtClaimType : Enumeration<JwtClaimType, string>
    {
        private const string _Sub = nameof(Sub);
        public static JwtClaimType Sub { get; } = new(_Sub);

        public JwtClaimType([NotNull] string name, string value) : base(name, value)
        {
        }

        public JwtClaimType(string value) : base(value)
        {
        }

    }
}
