
namespace Voxels.Objects
{
   public struct IntVector2
   {

      int _x, _z;

      public int X { get { return _x; } set { _x = value; } }
      public int Z { get { return _z; } set { _z = value; } }

      /// <summary>
      /// builds from given values
      /// </summary>
      /// <param name="x"></param>
      /// <param name="z"></param>
      public IntVector2(int x, int z)
      {
         _x = x;
         _z = z;
      }

      /// <summary>
      /// Builds copy of given intvector2
      /// </summary>
      /// <param name="pos"></param>
      public IntVector2(IntVector2 pos)
      {
         _x = pos.X;
         _z = pos.Z;
      }

      /// <summary>
      /// Builds from the given intvector3
      /// </summary>
      /// <param name="pos"></param>
      public IntVector2(IntVector3 pos)
      {
         _x = pos.X;
         _z = pos.Z;
      }

      /// <summary>
      /// Creates an IntVector3 by taking the lowest values from the given IntVector3 pair.
      /// </summary>
      /// <returns>The from.</returns>
      /// <param name="a">The a component.</param>
      /// <param name="b">The a component.</param>
      public static IntVector2 LowsFrom(IntVector2 a, IntVector2 b)
      {
         int newX = (a.X < b.X ? a.X : b.X);
         int newZ = (a.Z < b.Z ? a.Z : b.Z);
         return new IntVector2(newX, newZ);
      }

      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
      public override string ToString()
      {
         return "(" + X + ", " + Z + ")";
      }
   }
}