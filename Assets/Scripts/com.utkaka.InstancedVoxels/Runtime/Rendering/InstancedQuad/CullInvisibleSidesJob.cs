using com.utkaka.InstancedVoxels.Runtime.VoxelData;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace com.utkaka.InstancedVoxels.Runtime.Rendering.InstancedQuad {
	public struct CullInvisibleSidesJob : IJobFor {
		private readonly VoxelsBox _voxelsBox;
		private readonly int _sideMask;
		[ReadOnly]
		private NativeSlice<byte3> _inputIndices;
		[ReadOnly, NativeDisableParallelForRestriction]
		private NativeArray<byte> _voxelBoxMasks;
		
		[ReadOnly]
		private NativeArray<float3> _inputPositions;
		[ReadOnly]
		private NativeArray<float3> _inputColors;
		[ReadOnly]
		private NativeArray<uint> _inputBones;
		
		[WriteOnly]
		private NativeList<float3> _positions;
		[WriteOnly]
		private NativeList<float3> _colors;
		[WriteOnly]
		private NativeList<uint> _bones;

		public CullInvisibleSidesJob(VoxelsBox voxelsBox, int sideIndex, NativeSlice<byte3> inputIndices, NativeArray<byte> voxelBoxMasks, NativeArray<float3> inputPositions, NativeArray<float3> inputColors, NativeArray<uint> inputBones, NativeList<float3> positions, NativeList<float3> colors, NativeList<uint> bones) {
			_voxelsBox = voxelsBox;
			_sideMask = 1 << (sideIndex + 1);
			_inputIndices = inputIndices;
			_voxelBoxMasks = voxelBoxMasks;
			_inputPositions = inputPositions;
			_inputColors = inputColors;
			_inputBones = inputBones;
			_positions = positions;
			_colors = colors;
			_bones = bones;
		}

		public void Execute(int index) {
			var voxelIndex = _voxelsBox.GetExtendedVoxelIndex(_inputIndices[index]);
			if ((_voxelBoxMasks[voxelIndex] & _sideMask) == _sideMask) return;
			_positions.AddNoResize(_inputPositions[index]);
			_colors.AddNoResize(_inputColors[index]);
			_bones.AddNoResize(_inputBones[index]);
		}
	}
}