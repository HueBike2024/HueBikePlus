﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Core.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class SuccessMessage {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SuccessMessage() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Core.Properties.SuccessMessage", typeof(SuccessMessage).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tạo {0} thành công..
        /// </summary>
        public static string MSG_CREATE_SUCCESS {
            get {
                return ResourceManager.GetString("MSG_CREATE_SUCCESS", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Xóa {0} thành công..
        /// </summary>
        public static string MSG_DELETE_SUCCESS {
            get {
                return ResourceManager.GetString("MSG_DELETE_SUCCESS", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} succeeded..
        /// </summary>
        public static string MSG_SUCCESS {
            get {
                return ResourceManager.GetString("MSG_SUCCESS", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Đăng nhập thành công!.
        /// </summary>
        public static string MSG_SUCCESS_SIGN_IN {
            get {
                return ResourceManager.GetString("MSG_SUCCESS_SIGN_IN", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Đăng kí tài khoản thành công!.
        /// </summary>
        public static string MSG_SUCCESS_SIGN_UP {
            get {
                return ResourceManager.GetString("MSG_SUCCESS_SIGN_UP", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cập nhật {0} thành công..
        /// </summary>
        public static string MSG_UPDATE_SUCCESS {
            get {
                return ResourceManager.GetString("MSG_UPDATE_SUCCESS", resourceCulture);
            }
        }
    }
}
