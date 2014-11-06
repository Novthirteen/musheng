﻿using System;
using System.Collections.Generic;
using System.Linq;
using com.Sconit.CasClient.Authentication;

namespace com.Sconit.CasClient.Security
{
    /// <summary>
    /// Represents a CAS response to a validation request.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is the .Net port of org.jasig.cas.client.validation.Assertion
    /// </para>
    /// <para>
    /// Implementors should be Serializable
    /// </para>
    /// </remarks>
    /// <author>Scott Battaglia</author>
    /// <author>William G. Thompson, Jr. (.Net)</author>
    public interface IAssertion
    {
        /// <summary>
        /// The date from which this Assertion is valid.
        /// </summary>
        DateTime ValidFromDate { get; }

        /// <summary>
        /// The date this Assertion is valid until.
        /// </summary>
        DateTime ValidUntilDate { get; }

        /// <summary>
        /// The key/value pairs for attributes associated with this Assertion.
        /// </summary>
        ILookup<string, IList<string>> Attributes { get; }

        /// <summary>
        /// The name of the Principal that this Assertion backs.
        /// </summary>
        string PrincipalName { get; }
    }
}
