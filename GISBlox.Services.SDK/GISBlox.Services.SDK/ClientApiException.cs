// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using System;
using System.Net;

namespace GISBlox.Services.SDK
{
   /// <summary>
   /// Represents client-side API errors that occur during application execution.
   /// </summary>
   /// <remarks>
   /// Initializes a new instance of the GISBlox.Services.SDK.ClientApiException class.
   /// </remarks>
   /// <param name="message">The exception message.</param>
   /// <param name="statusCode">The HTTP status code.</param>
   public class ClientApiException(string message, HttpStatusCode statusCode) : Exception(message)
   {
      /// <summary>
      /// The HTTP status code of the error.
      /// </summary>
      public HttpStatusCode StatusCode { get; } = statusCode;
   }
}
