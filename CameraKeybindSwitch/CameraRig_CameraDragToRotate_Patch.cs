using Kingmaker.View;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using UnityEngine;
using HarmonyLib;

namespace CameraKeybindSwitch
{
	[HarmonyPatch(typeof(CameraRig), nameof(CameraRig.CameraDragToRotate))]
	public static class CameraRig_CameraDragToRotate_Patch
	{
		static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
		{
			List<CodeInstruction> newInstructions = new List<CodeInstruction>();
			MethodInfo methodInfoLeftAlt = SymbolExtensions.GetMethodInfo(() => UnityEngine.Input.GetKey(KeyCode.LeftAlt));
			MethodInfo methodInfoRightAlt = SymbolExtensions.GetMethodInfo(() => UnityEngine.Input.GetKey(KeyCode.RightAlt));

			List<int> retIndex = new List<int>();
			int bmpValueBoolIndex = 0;
			int leftAltBoolIndex = 0;
			int rightAltBoolIndex = 0;

			foreach (CodeInstruction instruction in instructions)
			{
				if (instruction.opcode == OpCodes.Ret)
				{
					retIndex.Add(newInstructions.Count);
				}
				if (instruction.ToString() == "brtrue.s Label0")
				{
					bmpValueBoolIndex = newInstructions.Count;
					newInstructions.Add(instruction);

					newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4, 308));
					newInstructions.Add(new CodeInstruction(OpCodes.Call, methodInfoLeftAlt));
					leftAltBoolIndex = newInstructions.Count;
					newInstructions.Add(new CodeInstruction(OpCodes.Brtrue_S));

					newInstructions.Add(new CodeInstruction(OpCodes.Ldc_I4, 307));
					newInstructions.Add(new CodeInstruction(OpCodes.Call, methodInfoRightAlt));
					rightAltBoolIndex = newInstructions.Count;
					newInstructions.Add(new CodeInstruction(OpCodes.Brtrue_S));
				}
				else
				{
					newInstructions.Add(instruction);
				}
			}

			newInstructions[retIndex[0] - 1].labels.Add(generator.DefineLabel());
			Label returnLabel = newInstructions[retIndex[0] - 1].labels[0];
			Label jumpLabel = newInstructions[retIndex[0] + 1].labels[0];


			if (bmpValueBoolIndex > 0)
			{
				newInstructions[bmpValueBoolIndex] = new CodeInstruction(OpCodes.Brfalse_S, returnLabel);
			}
			if (leftAltBoolIndex > 0)
			{
				newInstructions[leftAltBoolIndex] = new CodeInstruction(OpCodes.Brtrue_S, jumpLabel);
			}
			if (rightAltBoolIndex > 0)
			{
				newInstructions[rightAltBoolIndex] = new CodeInstruction(OpCodes.Brtrue_S, jumpLabel);
			}

			return newInstructions.AsEnumerable();
		}
	}
}
