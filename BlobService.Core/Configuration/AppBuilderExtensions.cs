﻿using BlobService.Core.Services;
using BlobService.Core.Stores;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using System;

namespace BlobService.Core.Configuration
{
    public static class AppBuilderExtensions
    {
        public static IApplicationBuilder UseBlobService(this IApplicationBuilder app)
        {
            app.Validate();

            app.UseMvc();

            return app;
        }

        internal static void Validate(this IApplicationBuilder app)
        {
            var loggerFactory = app.ApplicationServices.GetService(typeof(ILoggerFactory)) as ILoggerFactory;
            if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));

            var logger = loggerFactory.CreateLogger("BlobService.Startup");

            TestService(app.ApplicationServices, typeof(IStorageService), logger, "No storage mechanism for grants specified.");
            TestService(app.ApplicationServices, typeof(IBlobStore), logger, "No storage mechanism for Blob specified.");
            TestService(app.ApplicationServices, typeof(IContainerStore), logger, "No storage mechanism for Container specified.");
            TestService(app.ApplicationServices, typeof(ISASStore), logger, "No storage mechanism for SAS specified.", false);

        }

        internal static object TestService(IServiceProvider serviceProvider, Type service, ILogger logger, string message = null, bool doThrow = true)
        {
            var appService = serviceProvider.GetService(service);

            if (appService == null)
            {
                var error = message ?? $"Required service {service.FullName} is not registered in the DI container. Aborting startup";

                logger.LogCritical(error);

                if (doThrow)
                {
                    throw new InvalidOperationException(error);
                }
            }

            return appService;
        }
    }
}
