
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CMS.Implementation
{
    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    [global::System.ComponentModel.Composition.Export(typeof(global::Microsoft.LightSwitch.ClientGenerated.IClientApplicationFactory))]
    [global::System.ComponentModel.Composition.PartCreationPolicy(global::System.ComponentModel.Composition.CreationPolicy.Shared)]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "10.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public sealed class ApplicationFactory
        : global::Microsoft.LightSwitch.ClientGenerated.Implementation.ClientApplicationFactory
    {
        protected override global::Microsoft.LightSwitch.Client.IClientApplication CreateApplication(global::Microsoft.LightSwitch.Model.IApplicationDefinition applicationDefinition)
        {
            return new global::CMS.Application(applicationDefinition);
        }
    }

    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    [global::System.ComponentModel.Composition.Export(typeof(global::Microsoft.LightSwitch.IModuleFactory))]
    [global::System.ComponentModel.Composition.PartCreationPolicy(global::System.ComponentModel.Composition.CreationPolicy.Shared)]
    [global::Microsoft.LightSwitch.Framework.Base.ModuleFactory("CMS")]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "10.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public sealed class ModuleFactory
        : global::Microsoft.LightSwitch.Framework.Base.ModuleFactory
    {
        public override global::Microsoft.LightSwitch.IModuleBase GetModule(global::Microsoft.LightSwitch.Model.IModuleDefinition moduleDefinition)
        {
            return global::CMS.Application.Current;
        }
    }
}