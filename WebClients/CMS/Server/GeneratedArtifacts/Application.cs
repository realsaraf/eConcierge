
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CMS
{
    public sealed partial class Application
        : global::Microsoft.LightSwitch.Framework.Server.ServerApplication<global::CMS.Application, global::CMS.Application.DetailsClass, global::CMS.DataWorkspace>
    {

        public Application(global::Microsoft.LightSwitch.Model.IApplicationDefinition applicationDefinition, global::Microsoft.LightSwitch.Threading.IDispatcher logicDispatcher) : base(applicationDefinition, logicDispatcher)
        {
        }
        partial void Application_Initialize();

        public static new global::CMS.Application Current
        {
            get
            {
                return (global::CMS.Application)global::Microsoft.LightSwitch.Framework.Server.ServerApplication<global::CMS.Application, global::CMS.Application.DetailsClass>.Current;
            }
        }

        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "10.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public sealed class DetailsClass
            : global::Microsoft.LightSwitch.Details.Framework.Server.ServerApplicationDetails<global::CMS.Application, global::CMS.Application.DetailsClass, global::CMS.Application.DetailsClass.PropertySet, global::CMS.Application.DetailsClass.CommandSet, global::CMS.Application.DetailsClass.MethodSet>
        {
            static DetailsClass()
            {
            }

            [global::System.Diagnostics.DebuggerBrowsable(global::System.Diagnostics.DebuggerBrowsableState.Never)]
            private static readonly global::Microsoft.LightSwitch.Details.Framework.Base.ApplicationDetails<global::CMS.Application, global::CMS.Application.DetailsClass>.Entry
                __ApplicationEntry = new global::Microsoft.LightSwitch.Details.Framework.Base.ApplicationDetails<global::CMS.Application, global::CMS.Application.DetailsClass>.Entry(
                    a => a.Application_Initialize());

            public DetailsClass() : base()
            {
            }

            public new global::CMS.Application.DetailsClass.PropertySet Properties
            {
                get
                {
                    return base.Properties;
                }
            }

            public new global::CMS.Application.DetailsClass.CommandSet Commands
            {
                get
                {
                    return base.Commands;
                }
            }

            public new global::CMS.Application.DetailsClass.MethodSet Methods
            {
                get
                {
                    return base.Methods;
                }
            }


            [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "10.0.0.0")]
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
            public sealed class PropertySet
                : global::Microsoft.LightSwitch.Details.Framework.Server.ServerApplicationPropertySet<global::CMS.Application, global::CMS.Application.DetailsClass>
            {
            }

            [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "10.0.0.0")]
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
            public sealed class CommandSet
                : global::Microsoft.LightSwitch.Details.Framework.Server.ServerApplicationCommandSet<global::CMS.Application, global::CMS.Application.DetailsClass>
            {

            }

            [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "10.0.0.0")]
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
            public sealed class MethodSet
                : global::Microsoft.LightSwitch.Details.Framework.Server.ServerApplicationMethodSet<global::CMS.Application, global::CMS.Application.DetailsClass>
            {

            }

            [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "10.0.0.0")]
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
            internal sealed class PropertySetProperties
            {
            }

            [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "10.0.0.0")]
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
            internal sealed class CommandSetProperties
            {

            }

            [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "10.0.0.0")]
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
            internal sealed class MethodSetProperties
            {
            }
        }
    }
}
