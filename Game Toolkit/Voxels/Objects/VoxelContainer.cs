using System.Collections.Generic;
using System.Linq;

namespace Voxels.Objects
{
    public delegate void VoxelContainerEvent(VoxelContainer container);

    public class VoxelContainer
    {
		protected Dictionary<IntVector3, Voxel> Voxels;

        public IntVector3 Start { get; set; }
        public IntVector3 End { get; set; }

        public event VoxelContainerEvent Updated;

        public VoxelContainer()
        {
			Voxels = new Dictionary<IntVector3, Voxel>();
        }

        public VoxelContainer(IntVector3 start, IntVector3 end) : this()
        {
            Start = start;
            End = end;
        }

        public virtual Voxel GetVoxel(IntVector3 pos)
        {
			Voxel result = Voxel.Empty;
			Voxels.TryGetValue (pos, out result);
			return result;
        }

        public virtual void SetVoxel(IntVector3 pos, Voxel voxel)
        {
			lock (Voxels)
            {
                if (voxel != Voxel.Empty)
					Voxels[pos] = voxel;
				else
					Voxels.Remove(pos);
            }

            if((pos.X < Start.X || pos.Y < Start.Y || pos.Z < Start.Z) 
                || 
               (pos.X > End.X   || pos.Y > End.Y   || pos.Z > End.Z))
                UpdateBounds(pos, pos);

            if (Updated != null)
                Updated(this);
        }

        public virtual void SetVoxels(Dictionary<IntVector3, Voxel> changes)
        {
            IntVector3 lowestChange = changes.Keys.First();
            IntVector3 highestChange = changes.Keys.First();
			lock (Voxels)
            {
                foreach (var c in changes)
                {
                    if (c.Value != Voxel.Empty)
						Voxels[c.Key] = c.Value;
					else if (Voxels.ContainsKey(c.Key))
						Voxels.Remove(c.Key);

                    lowestChange = IntVector3.LowsFrom(lowestChange, c.Key);
                    highestChange = IntVector3.HighsFrom(highestChange, c.Key);
                }
            }

            UpdateBounds(lowestChange, highestChange);

            if (Updated != null)
                Updated(this);
        }

        public void UpdateBounds(IntVector3 lowestChange, IntVector3 highestChange)
        {
			lock (Voxels)
            {
                if (Voxels.Any())
                {
                    Start = IntVector3.LowsFrom(lowestChange, Start); ;
                    End = IntVector3.HighsFrom(highestChange, End); ;
                }
                else
                {
                    Start = IntVector3.Zero;
                    End = IntVector3.Zero;
                }
            }
        }

        public override string ToString()
        {
            int neg = 0, pos = 0, nil = 0;
            foreach (Voxel v in Voxels.Values)
            {
                if (v.Weight < 0)
                    neg++;
                else if (v.Weight > 0)
                    pos++;
                else
                    nil++;
            }

            return Start.ToString() + " - " + End.ToString() + " =0:" + nil + " -0: " + neg + ", +0: " + pos;
        }
    }
}
