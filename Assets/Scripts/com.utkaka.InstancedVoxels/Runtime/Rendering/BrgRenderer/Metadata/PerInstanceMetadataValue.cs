using UnityEngine.Rendering;

namespace com.utkaka.InstancedVoxels.Runtime.Rendering.BrgRenderer.Metadata {
    public class PerInstanceMetadataValue<T> : BatchMetadataValue<T> where T : unmanaged {
        public PerInstanceMetadataValue(string shaderProperty) : base(shaderProperty) { }

        public override int GetBufferSizeInFloat(int instanceCount) {
            return SizeInFloat * instanceCount;
        }

        public override MetadataValue GetMetadataValue(ref int offset, int instanceCount) {
            var value = new MetadataValue {
                NameID = Id,
                Value = (uint)offset | 0x80000000
            };
            offset += instanceCount * SizeInFloat;
            return value;
        }
    }
}