// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
// All other rights reserved.

using System;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;
using System.ComponentModel.Design;


#region Assembly Resource Attribute
[assembly: System.Web.UI.WebResource("AjaxControlToolkit.Common.Common.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("AjaxControlToolkit.Common.Common.debug.js", "text/javascript")]

#endregion


namespace AjaxControlToolkit
{    
    /// <summary>
    /// This class just exists as a type to get common scripts loaded.  For further info
    /// see Common.js in this folder.
    /// </summary>
    [ClientScriptResource(null, "AjaxControlToolkit.Common.Common.js")]
    public static class CommonToolkitScripts
    {        
    }
}


