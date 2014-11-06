// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
// All other rights reserved.

using System.Diagnostics.CodeAnalysis;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit.Design;

namespace AjaxControlToolkit
{
    /// <summary>
    /// Designer for AnimationExtender
    /// </summary>
    [SuppressMessage("Microsoft.Security", "CA2117:AptcaTypesShouldOnlyExtendAptcaBaseTypes", Justification = "Security handled by base class")]
    [TargetControlType(typeof(WebControl))]
    public class AnimationExtenderDesigner : ExtenderControlBaseDesigner<AnimationExtender>
    {
    }
}
