using UnityEngine;

namespace Dog.Game
{
	public class CastShadow : MonoBehaviour
	{
		private void Start()
		{
			foreach (var renderer in GetComponentsInChildren<MeshRenderer>())
			{
				var instance = Instantiate(renderer.material);
				instance.SetShaderPassEnabled("ShadowCaster", true);
				renderer.material = instance;
			}
		}
	}
}
