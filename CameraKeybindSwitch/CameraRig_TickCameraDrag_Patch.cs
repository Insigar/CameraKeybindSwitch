using HarmonyLib;
using Kingmaker.View;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace CameraKeybindSwitch
{
	[HarmonyPatch(typeof(CameraRig), nameof(CameraRig.TickCameraDrag))]

	class CameraRig_TickCameraDrag_Patch
	{
		static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
		{
			List<CodeInstruction> newInstructions = new List<CodeInstruction>(instructions);

			for (int i = 0; i <= 7; i++)
			{
				newInstructions[i].opcode = OpCodes.Nop;
			}

			return newInstructions.AsEnumerable();
		}
	}
}
