//------------------------------------------------------------------------------
// Copyright (c) 2003-2009 Whidsoft Corporation. All Rights Reserved.
//------------------------------------------------------------------------------

namespace Whidsoft.WebControls
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Implements the DescriptionAttribute except that the parameter is a resource name.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple=false, Inherited=true)]
    internal class ResDescriptionAttribute : DescriptionAttribute
    {
        /// <summary>
        /// Initializes a new instance of a ResDescriptionAttribute.
        /// </summary>
        /// <param name="resourceName">The name of the string resource.</param>
        public ResDescriptionAttribute(string resourceName): base()
        {
            DescriptionValue = Util.GetStringResource(resourceName);
        }
    }
}
