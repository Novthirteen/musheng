// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace AjaxControlToolkit
{
    /// <summary>
    /// Describes an object that can be used to resolve references to a control by its ID
    /// </summary>
    public interface IControlResolver
    {
        /// <summary>
        /// Resolves a reference to a control by its ID
        /// </summary>
        /// <param name="controlID"></param>
        /// <returns></returns>
        Control ResolveControl(string controlId);
    }
}
