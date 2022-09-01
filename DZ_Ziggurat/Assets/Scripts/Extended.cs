using System;
using RotaryHeart.Lib.SerializableDictionary;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ziggurat
{
	public enum EStateType : byte
	{
		Move = 0,
		FastAttack = 1,
		StrongAttack = 2,
		Die = 3
	}

	[Flags]
	public enum EIgnoreAxisType
	{
		None = 0,
		X = 1,
		Y = 2,
		Z = 4
	}
	
	public enum EUnitType
	{
		Blue,
		Red,
		Green,
	}

	[System.Serializable]
	public class AnimationKeyDictionary : SerializableDictionaryBase<EStateType, string> { }
}
