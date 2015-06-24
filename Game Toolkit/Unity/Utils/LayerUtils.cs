using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LayerUtils {

	//Unity does not have a built in list of layers
	//It just has all the layers, 0 through 31, even if they haven't been named
	//We'll just create our own list here
	public static Dictionary<string, int> GetLayers() {
		Dictionary<string, int> layers = new Dictionary<string, int>();
		for(int layer = 0; layer < 32; layer++) {
			string layerName = LayerMask.LayerToName(layer);
			if(layerName.Length > 0)
				layers.Add(layerName, layer);
		}
		return layers;
	}
}
