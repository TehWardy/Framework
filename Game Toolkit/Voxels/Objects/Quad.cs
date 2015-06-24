
namespace Voxels.Objects
{
   public struct Quad
   {
      public IntVector3 Min { get; set; }
      public IntVector3 Max { get; set; }
      public Face Face { get; set; }

      public bool Contains(IntVector3 pos)
      {
         bool withinX = pos.X >= Min.X && pos.X <= Max.X;
         bool withinY = pos.Y >= Min.Y && pos.Y <= Max.Y;
         bool withinZ = pos.Z >= Min.Z && pos.Z <= Max.Z;

         return withinX && withinY && withinZ;
      }

      public bool Contains(Quad quad)
      {
         return Contains(quad.Min) || Contains(quad.Max);
      }
   }
}