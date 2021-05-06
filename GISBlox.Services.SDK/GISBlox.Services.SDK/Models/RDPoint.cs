namespace GISBlox.Services.SDK.Models
{
   /// <summary>
   /// This class represents a pair of integer x- and y-coordinates that define a point in the Rijksdriehoeksstelsel coordinate system (EPSG:28992).
   /// </summary>
   public class RDPoint
   {
      /// <summary>
      /// Gets or sets the x-coordinate of the point in RDNew projection (EPSG:28992).
      /// </summary>
      public int X { get; set; }

      /// <summary>
      /// Gets or sets the y-coordinate of the point in RDNew projection (EPSG:28992).
      /// </summary>
      public int Y { get; set; }

      /// <summary>
      /// Initializes a new instance of the GISBlox.Services.SDK.Models.RDPoint class.
      /// </summary>
      public RDPoint()
      { }

      /// <summary>
      /// Initializes a new instance of the GISBlox.Services.SDK.Models.RDPoint class.
      /// </summary>
      /// <param name="x">The x-coordinate of the point in RDNew projection (EPSG:28992).</param>
      /// <param name="y">The y-coordinate of the point in RDNew projection (EPSG:28992).</param>
      public RDPoint(int x, int y)
      {
         this.X = x;
         this.Y = y;
      }
   }
}
