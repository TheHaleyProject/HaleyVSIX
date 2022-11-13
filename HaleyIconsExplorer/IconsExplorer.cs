using Microsoft.VisualStudio.Shell;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.IO;
using System.Linq;
using Haley.Models;
using Haley.MVVM;
using Haley.Abstractions;
using Haley.Events;
using Haley.Enums;
using Haley.Utils;
using HaleyIconsExplorer.ViewModels;
using HaleyIconsExplorer.Views;
using HaleyIconsExplorer.Enums;

namespace HaleyIconsExplorer {
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
    [Guid("e18559d8-86ba-4d27-82e8-f019b06abe25")]
    public class IconsExplorer : ToolWindowPane {
        string _basePath; 
        /// <summary>
        /// Initializes a new instance of the <see cref="IconsExplorer"/> class.
        /// </summary>
        public IconsExplorer() : base(null) {
            this.Caption = "IconsExplorer";
            _basePath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().Location, UriKind.RelativeOrAbsolute).LocalPath);
            var dirinfo = new DirectoryInfo(_basePath);

            AppDomain.CurrentDomain.AssemblyResolve += (s,a) =>  OnAssemblyResolve(a, dirinfo, false);
            InitializeContainerStore();
            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            this.Content = new IconsExplorerControl();
        }

        void InitializeContainerStore() {
            //Let the viewmodel be singleton
            ContainerStore.DI.Register<IconsExplorerVM>(); //Register as singleton
            ContainerStore.Controls.RegisterWithKey<IconsExplorerVM, BootStrapPage>(ViewEnum.BootStrap);
            ContainerStore.Controls.RegisterWithKey<IconsExplorerVM, BrandPage>(ViewEnum.Brand);
            ContainerStore.Controls.RegisterWithKey<IconsExplorerVM, HomePage>(ViewEnum.Home);
            ContainerStore.Controls.RegisterWithKey<IconsExplorerVM, InternalPage>(ViewEnum.Internal);
            ContainerStore.Controls.RegisterWithKey<IconsExplorerVM, FAwePage>(ViewEnum.FontAwe);
        }

        Assembly OnAssemblyResolve(ResolveEventArgs args, DirectoryInfo directory, bool isreflectionOnlyLoad) {
            //Below are only for loading the dll's which are not resolved. So, no need to load via the bytes. We can directly load the dll.
            try {
                Assembly loadedAssembly = null;
                Func<Assembly, bool> getAssemblyPredicate = (Assembly asm) => { return string.Equals(asm.FullName, args.Name, StringComparison.OrdinalIgnoreCase); };

                //Case 1 : If it exists already in the Domain, return it.
                if (isreflectionOnlyLoad) {
                    loadedAssembly = AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies().FirstOrDefault(
              asm => getAssemblyPredicate(asm));
                } else {
                    loadedAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(
              asm => getAssemblyPredicate(asm));
                }

                if (loadedAssembly != null) {
                    return loadedAssembly;
                }

                //Case 2: Doesn't exist in domain.So, check if the file exists.
                AssemblyName assemblyName = new AssemblyName(args.Name);
                string dependentAssemblyFilename = Path.Combine(directory.FullName, assemblyName.Name + ".dll");
                if (File.Exists(dependentAssemblyFilename)) {
                    if (isreflectionOnlyLoad) {
                        return Assembly.ReflectionOnlyLoadFrom(dependentAssemblyFilename);
                    } else {
                        //Assembly.Load will always look in to the default codebase. so we cannot refer other folders.
                        return Assembly.LoadFrom(dependentAssemblyFilename); //Not loading the bytes.
                    }
                }
                return null;
                //return Assembly.ReflectionOnlyLoad(args.Name);
            } catch (Exception ex) {
                throw ex;
            }
        }
    }
}
