using HarmonyLib;
using Kingmaker.View;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using UnityModManagerNet;

namespace CameraKeybindSwitch
{
	public class Main
	{
        public static bool Enabled;

		public static UnityModManager.ModEntry.ModLogger logger;

		static bool Load(UnityModManager.ModEntry modEntry)
        {
			logger = modEntry.Logger;
			modEntry.OnToggle = OnToggle;
            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            return true;
        }
        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            Enabled = value;
            return true;
        }
	}
}
