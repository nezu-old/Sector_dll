using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sector_dll.sdk
{
	[Obfuscation(Exclude = false, Feature = "+rename(mode=ascii,forceRen=true,renEnum=true);")]
	public enum TeamType : byte
	{
		Aegis,
		Helix,
		None,
		Bandit,
		Spectate,
		Infected
	}
}
