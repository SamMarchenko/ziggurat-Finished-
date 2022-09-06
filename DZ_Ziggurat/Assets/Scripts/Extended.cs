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
		Attack = 1,
		Die = 2
	}

	public enum EAttackState
	{
		FastAttack = 1,
		SlowAttack = 2
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
