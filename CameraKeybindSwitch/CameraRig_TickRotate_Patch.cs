using HarmonyLib;
using Kingmaker.View;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace CameraKeybindSwitch
{
	[HarmonyPatch(typeof(CameraRig), nameof(CameraRig.TickRotate))]

	class CameraRig_TickRotate_Patch
	{
		static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
		{
			List<CodeInstruction> newInstructions = new List<CodeInstruction>(instructions);
			MethodInfo methodInfoLeftAlt = SymbolExtensions.GetMethodInfo(() => UnityEngine.Input.GetKey(KeyCode.LeftAlt));
			MethodInfo methodInfoRightAlt = SymbolExtensions.GetMethodInfo(() => UnityEngine.Input.GetKey(KeyCode.RightAlt));

			Label ldargLabel0 = generator.DefineLabel();
			newInstructions[0].labels = new List<Label>() { ldargLabel0 };
			Label lastRetLabel = generator.DefineLabel();
			newInstructions[newInstructions.Count - 1].labels = new List<Label>() { lastRetLabel };

			List<CodeInstruction> addedCodes = new List<CodeInstruction>()
			{
				new CodeInstruction(OpCodes.Ldc_I4, 308),
				new CodeInstruction(OpCodes.Call, methodInfoLeftAlt),
				new CodeInstruction(OpCodes.Brtrue_S, ldargLabel0),
				new CodeInstruction(OpCodes.Ldc_I4, 307),
				new CodeInstruction(OpCodes.Call, methodInfoRightAlt),
				new CodeInstruction(OpCodes.Brfalse, lastRetLabel)
			};

			newInstructions.InsertRange(0, addedCodes);

			return newInstructions.AsEnumerable();
		}
	}
}
