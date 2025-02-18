using HarmonyLib;
using Kingmaker.View;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace CameraKeybindSwitch
{
	[HarmonyPatch(typeof(CameraRig), nameof(CameraRig.CameraDragToMove))]
	class CameraRig_CameraDragToMove_Patch
	{
		static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
		{
			List<CodeInstruction> newInstructions = new List<CodeInstruction>(instructions);

			newInstructions.Insert(8, newInstructions[62]);
			newInstructions[8].labels.Clear();
			newInstructions.RemoveAt(63);
			newInstructions.Insert(9, newInstructions[63]);
			newInstructions.RemoveAt(64);

			Label label0 = generator.DefineLabel();
			Label label1 = generator.DefineLabel();

			newInstructions[2] = new CodeInstruction(OpCodes.Brtrue_S, label0);
			newInstructions[5] = new CodeInstruction(OpCodes.Brfalse_S, label1);
			newInstructions[6].labels = new List<Label>() { label0 };
			newInstructions[7] = new CodeInstruction(OpCodes.Brtrue_S, label1);
			newInstructions[10].labels = new List<Label>() { label1 };

			return newInstructions.AsEnumerable();
		}
	}
}
