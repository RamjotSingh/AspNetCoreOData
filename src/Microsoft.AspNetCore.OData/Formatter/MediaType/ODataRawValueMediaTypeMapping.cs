﻿// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.OData.UriParser;

namespace Microsoft.AspNetCore.OData.Formatter.MediaType
{
    /// <summary>
    /// Media type mapping that associates requests with $count.
    /// </summary>
    /// <remarks>This class derives from a platform-specific class.</remarks>
    public abstract class ODataRawValueMediaTypeMapping : MediaTypeMapping
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ODataRawValueMediaTypeMapping"/> class.
        /// </summary>
        protected ODataRawValueMediaTypeMapping(string mediaType)
            : base(mediaType)
        {
        }

        /// <summary>
        /// This method determines if the <see cref="HttpRequest"/> is an OData Raw value request.
        /// </summary>
        /// <param name="propertySegment">The <see cref="PropertySegment"/> of the path.</param>
        /// <returns>True if the request is an OData raw value request.</returns>
        protected abstract bool IsMatch(PropertySegment propertySegment);

        internal static bool IsRawValueRequest(ODataPath path)
        {
            return path != null && path.LastSegment is ValueSegment;
        }

        private static PropertySegment GetProperty(ODataPath odataPath)
        {
            if (odataPath == null || odataPath.Count < 2)
            {
                return null;
            }

            return odataPath.ElementAt(odataPath.Count - 2) as PropertySegment;
        }

        /// <inheritdoc/>
        public override double TryMatchMediaType(HttpRequest request)
        {
            if (request == null)
            {
                throw Error.ArgumentNull("request");
            }

            ODataPath odataPath = request.ODataFeature().Path;
            return (IsRawValueRequest(odataPath) && IsMatch(GetProperty(odataPath))) ? 1 : 0;
        }
    }
}
