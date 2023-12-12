using System.Collections;
using com.utkaka.InstancedVoxels.Runtime.Rendering;
using com.utkaka.InstancedVoxels.Runtime.VoxelData;
using Unity.PerformanceTesting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace com.utkaka.InstancedVoxels.Tests {
	public abstract class VoxelRendererPerformanceTest<T> where T : MonoBehaviour, IVoxelRenderer {
		protected IEnumerator Test(string voxelSize) {
			var sampleGroups = new []{
				new SampleGroup("PlayerLoop", SampleUnit.Microsecond), 
				//new SampleGroup("Gfx.WaitForPresentOnGfxThread", SampleUnit.Microsecond),
				new SampleGroup("GfxDeviceMetal.WaitForLastPresent", SampleUnit.Microsecond)
			};
			
			var cameraTransform = new GameObject("Main Camera", typeof(Camera)).transform;
			cameraTransform.gameObject.tag = "MainCamera";
			cameraTransform.position = new Vector3(0.38f, 1.17f, -1.44f);
			cameraTransform.rotation = Quaternion.Euler(32.06f, -12.81f, 0.0f);

			var renderer = new GameObject("Renderer", typeof(T))
				.GetComponent<T>();
			
			var voxels = Resources.Load<Voxels>($"Voxel_{voxelSize}");
			renderer.Init(voxels);
			yield return Measure.Frames()
				.WarmupCount(60)
				.ProfilerMarkers(sampleGroups)
				.MeasurementCount(60)
				.Run();
			Object.Destroy(renderer.gameObject);
			Object.Destroy(cameraTransform.gameObject);
		}
	}
}