using System;
using System.Collections.Generic;
using System.Text;
using Authorisation.Core.Services;
using Autofac;

namespace Authorisation.Core
{
    public class CoreModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ICognitiveFaceService>();

            builder.RegisterType<ICognitiveAdminService>();

            base.Load(builder);
        }

    }
}
