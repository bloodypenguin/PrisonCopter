using ColossalFramework;
using HarmonyLib;
using System;
using System.Reflection;

namespace PrisonHelicopter.PrisonHelicopter.HarmonyPatches {

    [HarmonyPatch(typeof(PrisonerAI))]
    public static class PrisonerAIPatch {

        [HarmonyPatch(typeof(PrisonerAI), "GetLocalizedStatus",
            new Type[] { typeof(ushort), typeof(CitizenInstance), typeof(InstanceID) },
            new ArgumentType[] { ArgumentType.Normal, ArgumentType.Ref, ArgumentType.Out })]
        [HarmonyPrefix]
        public static bool Prefix1(PrisonerAI __instance, ushort instanceID, ref CitizenInstance data, out InstanceID target, ref string __result)
	{
	    if ((data.m_flags & (CitizenInstance.Flags.Blown | CitizenInstance.Flags.Floating)) != 0)
	    {
		target = InstanceID.Empty;
		__result = ColossalFramework.Globalization.Locale.Get("CITIZEN_STATUS_CONFUSED");
	    }
            else
            {
                ushort targetBuilding = data.m_targetBuilding;
	        if (targetBuilding != 0)
	        {
		    target = InstanceID.Empty;
		    target.Building = targetBuilding;
		    __result = "Serving time at ";
	        }
                else
                {
                    target = InstanceID.Empty;
	            __result = ColossalFramework.Globalization.Locale.Get("CITIZEN_STATUS_CONFUSED");
                }
            }
	    return false;
	}

        [HarmonyPatch(typeof(PrisonerAI), "GetLocalizedStatus",
            new Type[] { typeof(uint), typeof(Citizen), typeof(InstanceID) },
            new ArgumentType[] { ArgumentType.Normal, ArgumentType.Ref, ArgumentType.Out })]
        [HarmonyPrefix]
        public static bool Prefix2(PrisonerAI __instance, uint citizenID, ref Citizen data, out InstanceID target, ref string __result)
	{
	    CitizenManager instance = Singleton<CitizenManager>.instance;
	    ushort instance2 = data.m_instance;
	    if (instance2 != 0)
	    {
                Prefix1(__instance, instance2, ref instance.m_instances.m_buffer[instance2], out target, ref __result);
	    }
	    else if (data.m_visitBuilding != 0)
	    {
		target = InstanceID.Empty;
		target.Building = data.m_visitBuilding;
		__result = "Serving time at ";
	    }
            else
            {
                target = InstanceID.Empty;
                __result = ColossalFramework.Globalization.Locale.Get("CITIZEN_STATUS_CONFUSED");
            }
	    return false;
	}
    }
}
