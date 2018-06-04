using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.VFX.Block;
using UnityEngine;
using UnityEngine.Experimental.VFX;

namespace UnityEditor.VFX
{
    [VFXInfo]
    class VFXDecalOutput : VFXAbstractParticleOutput
    {
        public override string name { get { return "Decal Output"; } }
        public override string codeGeneratorTemplate { get { return RenderPipeTemplate("VFXParticleDecal"); } }
        public override VFXTaskType taskType { get { return VFXTaskType.ParticleHexahedronOutput; } }
        public override bool supportsUV { get { return true; } }
        public override CullMode defaultCullMode { get { return CullMode.Back; } }

        public class InputProperties
        {
            public Texture2D mainTexture = VFXResources.defaultResources.particleTexture;
        }

        public override void OnEnable()
        {
            base.OnEnable();
            cullMode = CullMode.Back;
            zTestMode = ZTestMode.LEqual;
            zWriteMode = ZWriteMode.Off;
        }

        protected override IEnumerable<string> filteredOutSettings
        {
            get
            {
                foreach (var setting in base.filteredOutSettings)
                    yield return setting;

                yield return "cullMode";
                yield return "zWriteMode";
                yield return "zTestMode";
            }
        }

        protected override IEnumerable<VFXNamedExpression> CollectGPUExpressions(IEnumerable<VFXNamedExpression> slotExpressions)
        {
            foreach (var exp in base.CollectGPUExpressions(slotExpressions))
                yield return exp;

            yield return slotExpressions.First(o => o.name == "mainTexture");
        }

        public override IEnumerable<VFXAttributeInfo> attributes
        {
            get
            {
                yield return new VFXAttributeInfo(VFXAttribute.Position, VFXAttributeMode.Read);
                yield return new VFXAttributeInfo(VFXAttribute.Color, VFXAttributeMode.Read);
                yield return new VFXAttributeInfo(VFXAttribute.Alpha, VFXAttributeMode.Read);
                yield return new VFXAttributeInfo(VFXAttribute.Alive, VFXAttributeMode.Read);
                yield return new VFXAttributeInfo(VFXAttribute.AxisX, VFXAttributeMode.Read);
                yield return new VFXAttributeInfo(VFXAttribute.AxisY, VFXAttributeMode.Read);
                yield return new VFXAttributeInfo(VFXAttribute.AxisZ, VFXAttributeMode.Read);
                yield return new VFXAttributeInfo(VFXAttribute.AngleX, VFXAttributeMode.Read);
                yield return new VFXAttributeInfo(VFXAttribute.AngleY, VFXAttributeMode.Read);
                yield return new VFXAttributeInfo(VFXAttribute.AngleZ, VFXAttributeMode.Read);
                yield return new VFXAttributeInfo(VFXAttribute.Pivot, VFXAttributeMode.Read);

                foreach (var size in VFXBlockUtility.GetReadableSizeAttributes(GetData()))
                    yield return size;

                if (uvMode != UVMode.Simple)
                    yield return new VFXAttributeInfo(VFXAttribute.TexIndex, VFXAttributeMode.Read);
            }
        }
    }
}
