# GISBlox Services .NET SDK
This client library enables client applications to connect to the GISBlox Services REST API. The API currently supports reprojecting [WGS84](https://en.wikipedia.org/wiki/World_Geodetic_System#WGS84) coordinates to [Rijksdriehoeksstelsel](https://nl.wikipedia.org/wiki/Rijksdriehoeksco%C3%B6rdinaten) (RDNew) locations and vice versa, and converting [WKT](https://en.wikipedia.org/wiki/Well-known_text_representation_of_geometry) geometry objects into [GeoJson](https://en.wikipedia.org/wiki/GeoJSON).

## Requirements
You must have a personal service key to access the GISBlox Services REST API.

## Usage

Check out the Samples project for more detailed usages.

In the following examples, the ```baseUrl``` variable should be set to "https://services.gisblox.com", and the ```serviceKey``` variable should be set to your GISBlox Services personal service key.

### Creating the client

```cs
using (var client = GISBloxClient.CreateClient(baseUrl, serviceKey))
{
    // ...
}

```

### Conversion API
The conversion API converts WKT geometry objects into GeoJson.

```cs
// Convert a POINT geometry into a GeoJson Feature 
var wkt = new WKT("POINT (30 10)");
var geoJson = await client.Conversion.ToGeoJson(wkt);

// Convert a MULTIPOLYGON geometry into a GeoJson FeatureCollection
var wkt = new WKT("MULTIPOLYGON (((30 20, 45 40, 10 40, 30 20)),((15 5, 40 10, 10 20, 5 10, 15 5)))");
var geoJson = await client.Conversion.ToGeoJson(wkt, true);
```

### Projection API
The projection API currently reprojects WGS84 coordinates to Rijksdriehoeksstelsel (RDNew) locations and vice versa.

#### Reproject WGS84 to RDNew

```cs
// Reproject WGS84 coordinate
var coord = new Coordinate(51.998929, 4.375587);         
var rdPoint = await client.Projection.ToRDS(coord);

// Reproject multiple coordinates at once
var coords = new List<Coordinate>
{
  new Coordinate(51.998929, 4.375587),
  new Coordinate(53.1, 4.2),
  new Coordinate(53.11, 4.3)
};
var rdPoints = await client.Projection.ToRDS(coords);
```

#### Reproject RDNew to WGS84

```cs
// Reproject RDNew location
var rdPoint = new RDPoint(100000, 555000);         
var coord = await client.Projection.ToWGS84(rdPoint);

// Reproject multiple RDNew locations at once
var rdPoints = new List<RDPoint>
{
  new RDPoint(100000, 555000),
  new RDPoint(1, 2),
  new RDPoint(111000, 550000)
};
var coords = await client.Projection.ToWGS84(rdPoints);
```

To include the source coordinates in the result, call the ```ToRDSComplete``` or ```ToWGS84Complete``` methods respectively.

```cs
await client.Projection.ToRDSComplete(coords);
await client.Projection.ToWGS84Complete(rdPoints);
```

