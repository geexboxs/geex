using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Geex.Shared
{
    public abstract class GraphQLModule<T> where T : GraphQLModule<T>
    {

        /// <summary>Gets or sets the logger.</summary>
        public ILogger Logger { get; set; }

        protected GraphQLModule(ILogger<T> logger)
        {
            this.Logger = logger;
        }

        /// <summary>
        /// This is the first event called on application startup.
        /// Codes can be placed here to run before dependency injection registrations.
        /// </summary>
        public abstract void PreInitialize();

        /// <summary>
        /// This method is used to register dependencies for this module.
        /// </summary>
        public abstract void OnInitialize();

        /// <summary>This method is called lastly on application startup.</summary>
        public abstract void PostInitialize();
    }
}
