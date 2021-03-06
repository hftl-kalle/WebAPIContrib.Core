﻿using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;

namespace WebApiContrib.Core.Versioning
{
    /// <summary>
    /// This version strategy gets its version from a custom header.
    /// The header name can be configured using the <see cref="HeaderName"/> property.
    /// </summary>
    public class CustomHeaderVersionStrategy : IVersionStrategy
    {
        /// <summary>
        /// Gets or sets the custom header name to use for determining the version.
        /// </summary>
        /// <remarks>
        /// The default value is <c>Api-Version</c>
        /// </remarks>
        public string HeaderName { get; set; } = "Api-Version";

        /// <inheritdoc />
        VersionResult? IVersionStrategy.GetVersion(HttpContext context, RouteData routeData)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var headers = context.Request.Headers;

            if (headers.TryGetValue(HeaderName, out var header))
            {
                var headerString = header.ToString();

                if (ParsingUtility.TryParseVersion(headerString, out var version))
                {
                    return new VersionResult(version, HeaderName);
                }
            }

            return null;
        }
    }
}
