<h1 align="center">GISBlox Services SDK</h1>

<p align="center">
  <br>
  <img src="GBLS.png" alt="GISBlox Location Services logo" width="120px" height="120px"/>
  <br><br>
  <i>Connect to the GISBlox Services API from .NET</i>
  <br>
</p>

<p align="center">  
  <a href="https://github.com/GISBlox/gisblox-services-sdk/blob/main/LICENSE">
    <img src="https://img.shields.io/github/license/GISBlox/gisblox-services-sdk" alt="MIT license" />
  </a>
</p>

<hr>

## 🧑‍💻 Introduction

This SDK enables applications to connect to the [GISBlox Services API](https://services.gisblox.com/). It supports the following Location Services:

#### Conversion
  * Convert [WKB](https://en.wikipedia.org/wiki/Well-known_text_representation_of_geometry#Well-known_binary) and [WKT](https://en.wikipedia.org/wiki/Well-known_text_representation_of_geometry) geometry objects into [GeoJson](https://en.wikipedia.org/wiki/GeoJSON), and vice versa. 
  Supports both single-shot conversions, as well as batch processing of large collections.

#### Data Lake
  * Access the GISBlox GeoJson Data Lake: upload and manage your GeoJson datasets in the cloud. Supports exporting to modern data formats, such as GeoParquet.

#### Info
  * Quickly retrieve an overview of your subscriptions and their details.

#### Map Analytics
  * Analyze user behavior on interactive maps (see [Map Analytics](https://www.gisblox.com/map-analytics)) to provide clarity on engagement and map usage.

#### Postal codes
  * Query and visualize Dutch postal codes (as used by [ZipChat Copilot](https://www.gisblox.com/zipchat-copilot)) to turn raw postal code data into meaningful insights for regional analysis and decision-making.

#### Projection
  * Project [WGS84](https://en.wikipedia.org/wiki/World_Geodetic_System#WGS84) coordinates to [Rijksdriehoeksstelsel](https://nl.wikipedia.org/wiki/Rijksdriehoeksco%C3%B6rdinaten) (RDNew) locations, and vice versa. Includes batch processing capabilities. 

To get no-code access to the GISBlox Services API from the command-line, you can use the [GISBlox Services CLI](https://github.com/GISBlox/gisblox-services-cli).

## 🗝️ Requirements
You must have a personal service key to access the GISBlox Services API.

To generate a service key:

1. Sign up for a GISBlox account at [gisblox.com/signup](https://account.gisblox.com/registreren)
2. Navigate to your [Subscriptions Dashboard](https://account.gisblox.com/profiel/abonnementen)
3. Click the `Add` button and select the **free** subscription to the **GISBlox Location Services** in the dropdown.
4. Once subscribed, click the **Location Services** tile and copy the service key from the information panel.

Based on your requirements, you may need to add additional subscriptions to your account.


## Installation
Either download this repository, make a (forked) git clone or install via NuGet:

```
PM> Install-Package GISBlox.Services.SDK
```

## 📖 Usage

The `GISBloxClient` requires two parameters to be passed during initialization: the `baseUrl` variable should be set to `https://services.gisblox.com`, and the `serviceKey` variable should contain your personal service key.

### Creating the client

```cs
using (var client = GISBloxClient.CreateClient(baseUrl, serviceKey))
{
    // ...
}

```

The [Tests](/GISBlox.Services.SDK/GISBlox.Services.SDK.Tests) project contains detailed use cases for every available API endpoint.

## 🆘 Getting Support

- [Ask questions, share ideas](https://github.com/GISBlox/gisblox-services-sdk/discussions)
- [Create an issue](https://github.com/GISBlox/gisblox-services-sdk/issues)

