﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1433
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Atrendia.CourseManagement.UserRegistration {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Atrendia.CourseManagement.UserRegistration.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Your Access to Atrendia Course Management System.
        /// </summary>
        internal static string Email_Subject {
            get {
                return ResourceManager.GetString("Email_Subject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Dear {0},
        ///
        ///Below are your personal login credentials that enable you to
        ///effectively manage your company participation in Atrendia
        ///courses:
        ///
        ///	Address: http://dev.atrendia.com/CourseManagement/
        ///	Login email:  {1}
        ///	Use password: {2}
        ///
        ///To learn more about Atrendia offerings visit:
        ///
        ///	http://www.atrendia.com/
        ///
        ///If you have any questions, please contact us:
        ///
        ///	info@atrendia.com
        ///
        ///--
        ///Atrendia.
        /// </summary>
        internal static string Email_Text {
            get {
                return ResourceManager.GetString("Email_Text", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Your Access to Atrendia Course Management System.
        /// </summary>
        internal static string RegTrainerEmail_Subject {
            get {
                return ResourceManager.GetString("RegTrainerEmail_Subject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Dear {0},
        ///
        ///	Address: http://dev.atrendia.com/CourseManagement/
        ///	Login email:  {1}
        ///	Use password: {2}
        ///
        ///--
        ///Atrendia.
        /// </summary>
        internal static string RegTrainerEmail_Text {
            get {
                return ResourceManager.GetString("RegTrainerEmail_Text", resourceCulture);
            }
        }
    }
}
