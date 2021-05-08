// ------------------------------------------------------------
// Copyright (c) Bartels Online.  All rights reserved.
// ------------------------------------------------------------

using System;
using System.Net;

namespace GISBlox.Services.SDK
{
   public class ClientApiException : Exception
   {
      public HttpStatusCode StatusCode { get; }

      public ClientApiException(string message, HttpStatusCode statusCode) : base(message)
      {
         StatusCode = statusCode;
      }
   }
}
