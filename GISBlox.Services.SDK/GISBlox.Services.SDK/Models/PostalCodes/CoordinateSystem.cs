﻿// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

namespace GISBlox.Services.SDK.Models
{
    /// <summary>
    /// Represents a coordinate system.
    /// </summary>
    public enum CoordinateSystem
    {
        /// <summary>
        /// Rijksdriehoeksstelsel (EPSG:28992)
        /// </summary>
        RDNew = 28992,

        /// <summary>
        /// Latitude/longitude (EPSG:4326)
        /// </summary>
        WGS84 = 4326
    }
}
