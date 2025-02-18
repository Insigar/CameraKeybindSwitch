using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Kingmaker.View;
using HarmonyLib;

namespace CameraKeybindSwitch
{
	[HarmonyPatch(typeof(CameraRig), nameof(CameraRig.RotateByMiddleButton))]

	class CameraRig_RotateByMiddleButton_Patch
	{
		static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
		{
			List<CodeInstruction> newInstructions = new List<CodeInstruction>(instructions);

			newInstructions[2].operand = newInstructions[7].labels[0];
			newInstructions[5].opcode = OpCodes.Brtrue_S;

			return newInstructions.AsEnumerable();
		}
	}
}
