using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;

namespace TestZone
{
    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("fedd742c-51d6-473f-a4d9-eae4e6747e76")]
    public class TestWindow : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestWindow"/> class.
        /// </summary>
        public TestWindow() : base(null)
        {
            this.Caption = "TestWindow";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            this.Content = new TestWindowControl();
        }
    }
}
