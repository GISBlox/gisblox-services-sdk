<h1 align="center">GISBlox Services SDK</h1>

<p align="center">
  <br>
  <img src="GBLS.png" alt="GISBlox Location Services logo" width="150px" height="150px"/>
  <br><br>
  <i>The GISBlox Services .NET SDK enables applications to connect to the GISBlox Services API.</i>
  <br>
</p>

<p align="center">  
  <a href="https://github.com/GISBlox/gisblox-services-sdk/blob/main/LICENSE">
    <img src="https://img.shields.io/github/license/GISBlox/gisblox-services-sdk" alt="MIT license" />
  </a>
</p>

<hr>

## Introduction

This SDK enables applications to connect to the [GISBlox Services API](https://services.gisblox.com/). It supports the Location Services:
* Getting information on Dutch postal codes (used by [ZipChat Copilot](https://www.gisblox.com/zipchat-copilot))
* Reprojecting [WGS84](https://en.wikipedia.org/wiki/World_Geodetic_System#WGS84) coordinates to [Rijksdriehoeksstelsel](https://nl.wikipedia.org/wiki/Rijksdriehoeksco%C3%B6rdinaten) (RDNew) locations and vice versa
* Converting [WKT](https://en.wikipedia.org/wiki/Well-known_text_representation_of_geometry) geometry objects into [GeoJson](https://en.wikipedia.org/wiki/GeoJSON)
* Retrieving subscription information

To get no-code access to the GISBlox Services API from the command-line, you can use the [GISBlox Services CLI](https://github.com/GISBlox/gisblox-services-cli).

## Requirements
You must have a personal service key to access the GISBlox Services API.

To generate a service key, create an account in the [GISBlox Account Center](https://account.gisblox.com/) and add a **free** subscription to the GISBlox Location Services. Once subscribed, click the Location Services tile and copy the service key from the information panel. [More information](http://library.gisblox.com/content/nl-nl/gb1810090).

## Installation
Either download this repository, make a (forked) git clone or install via NuGet:

```
PM> Install-Package GISBlox.Services.SDK -Version 2.1.0
```

## Usage

Check out the [Tests](/GISBlox.Services.SDK/GISBlox.Services.SDK.Tests) project for more detailed use cases.

In the following examples, the ```baseUrl``` variable should be set to `https://services.gisblox.com`, and the ```serviceKey``` variable should be set to your GISBlox Location Services service key.

### Creating the client

```cs
using (var client = GISBloxClient.CreateClient(baseUrl, serviceKey))
{
    // ...
}

```

## Postal codes API
The postal codes API supports querying for both 4 digit and 6 digit postcal code records. Every endpoint in the ```Dutch Postal Codes``` sections of the [developer portal](https://services.gisblox.com/) is available in the SDK.

## Projection API
The projection API reprojects WGS84 coordinates to Rijksdriehoeksstelsel (RDNew) locations and vice versa.

### Reproject WGS84 coordinate to RDNew point

```cs
var coord = new Coordinate(51.998929, 4.375587);         
var rdPoint = await client.Projection.ToRDS(coord);

// Returns X:85530 Y:446100
```

Use the following code to reproject multiple WGS84 coordinates to RDNew locations at once:

```cs
var coords = new List<Coordinate>
{
  new Coordinate(51.998929, 4.375587),
  new Coordinate(53.1, 4.2),
  new Coordinate(53.11, 4.3)
};
var rdPoints = await client.Projection.ToRDS(coords);

// Returns X:85530 Y:446100, X:75483 Y:568787 and X:82197 Y:569794
```
To include the source location in the result, call the ```ToRDSComplete``` method instead of ```ToRDS```:

```cs
await client.Projection.ToRDSComplete(coord);

// Returns X:85530 Y:446100 Lat: 51,998929 Lon: 4,375587
```

### Reproject RDNew point to WGS84 coordinate

```cs
var rdPoint = new RDPoint(100000, 555000);         
var coord = await client.Projection.ToWGS84(rdPoint);

// Returns Lat: 52,9791861737104 Lon: 4,56833613045079
```

You can round the digits of the resulting coordinate to a specific amount by passing a second argument:

```cs
var coord = await client.Projection.ToWGS84(rdPoint, 6);    // Round the coordinate to 6 digits

// Returns Lat: 52,979186 Lon: 4,568336
```

Use the following code to reproject multiple RDNew locations to WGS84 coordinates at once:

```cs
var rdPoints = new List<RDPoint>
{
  new RDPoint(100000, 555000),
  new RDPoint(1, 2),
  new RDPoint(111000, 550000)
};
var coords = await client.Projection.ToWGS84(rdPoints);

// Returns Lat: 52,9791861737104 Lon: 4,56833613045079, Lat: 0 Lon: 0 and Lat: 52.93526683092437 Lon: 4.7327735938900535
```

To include the source coordinates in the result, call the ```ToWGS84Complete``` method instead of ```ToWGS84```:

```cs
await client.Projection.ToWGS84Complete(rdPoint);

// Returns X: 100000 Y: 555000 Lat: 52,9791861737104 Lon: 4,56833613045079
```

## Conversion API
The conversion API converts WKT geometry objects into GeoJson.

### Convert WKT into GeoJson

```cs
var wkt = new WKT("POINT (30 10)");
var geoJson = await client.Conversion.ToGeoJson(wkt);

// Returns:

{
  "type": "Feature",
  "geometry": {
    "type": "Point",
    "coordinates": [
      10,
      30
    ]
  },
  "properties": {}
}
```

The following code converts a multipolygon into a GeoJson FeatureCollection:

```cs
var wkt = new WKT("MULTIPOLYGON (((30 20, 45 40, 10 40, 30 20)),((15 5, 40 10, 10 20, 5 10, 15 5)))");
var geoJson = await client.Conversion.ToGeoJson(wkt, true);

// Returns:

{
  "type": "FeatureCollection",
  "features": [
    {
      "type": "Feature",
      "geometry": {
        "type": "MultiPolygon",
        "coordinates": [
          [
            [
              [
                20,
                30
              ],
              [
                40,
                45
              ],
              [
                40,
                10
              ],
              [
                20,
                30
              ]
            ]
          ],
          [
            [
              [
                5,
                15
              ],
              [
                10,
                40
              ],
              [
                20,
                10
              ],
              [
                10,
                5
              ],
              [
                5,
                15
              ]
            ]
          ]
        ]
      },
      "properties": {}
    }
  ]
}
```
