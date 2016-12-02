using System;
using UnityEngine;
using Prime31.GoKitLite;

namespace Prime31.GoKitLite
{
	public enum GoEaseType
	{
		Linear = 0,
		
		SineIn,
		SineOut,
		SineInOut,
		
		QuadIn,
		QuadOut,
		QuadInOut,
		
		CubicIn,
		CubicOut,
		CubicInOut,
		
		QuartIn,
		QuartOut,
		QuartInOut,
		
		QuintIn,
		QuintOut,
		QuintInOut,
		
		ExpoIn,
		ExpoOut,
		ExpoInOut,
		
		CircIn,
		CircOut,
		CircInOut,
		
		ElasticIn,
		ElasticOut,
		ElasticInOut,
		Punch,
		
		BackIn,
		BackOut,
		BackInOut,
		
		BounceIn,
		BounceOut,
		BounceInOut
	}

	public static class GoKitHelpers 
	{
		static Func<float,float,float>[] funcs = new Func<float, float, float>[32]{
			GoKitLiteEasing.Linear.EaseNone,

			GoKitLiteEasing.Sinusoidal.EaseIn,
			GoKitLiteEasing.Sinusoidal.EaseOut,
			GoKitLiteEasing.Sinusoidal.EaseInOut,

			GoKitLiteEasing.Quadratic.EaseIn,
			GoKitLiteEasing.Quadratic.EaseOut,
			GoKitLiteEasing.Quadratic.EaseInOut,

			GoKitLiteEasing.Cubic.EaseIn,
			GoKitLiteEasing.Cubic.EaseOut,
			GoKitLiteEasing.Cubic.EaseInOut,

			GoKitLiteEasing.Quartic.EaseIn,
			GoKitLiteEasing.Quartic.EaseOut,
			GoKitLiteEasing.Quartic.EaseInOut,
			
			GoKitLiteEasing.Quintic.EaseIn,
			GoKitLiteEasing.Quartic.EaseOut,
			GoKitLiteEasing.Quartic.EaseInOut,

			GoKitLiteEasing.Exponential.EaseIn,
			GoKitLiteEasing.Exponential.EaseOut,
			GoKitLiteEasing.Exponential.EaseInOut,

			GoKitLiteEasing.Circular.EaseIn,
			GoKitLiteEasing.Circular.EaseOut,
			GoKitLiteEasing.Circular.EaseInOut,

			GoKitLiteEasing.Elastic.EaseIn,
			GoKitLiteEasing.Elastic.EaseOut,
			GoKitLiteEasing.Elastic.EaseInOut,
			GoKitLiteEasing.Elastic.Punch,

			GoKitLiteEasing.Back.EaseIn,
			GoKitLiteEasing.Back.EaseOut,
			GoKitLiteEasing.Back.EaseInOut,

			GoKitLiteEasing.Bounce.EaseIn,
			GoKitLiteEasing.Bounce.EaseOut,
			GoKitLiteEasing.Bounce.EaseInOut
		};

		/// <summary>
		/// fetches the actual function for the given ease type
		/// </summary>
		public static Func<float,float,float> EaseFunctionForType(GoEaseType easeType)
		{
			return funcs[(int)easeType];
//			switch(easeType)
//			{
//				case GoEaseType.Linear:
//					return GoKitLiteEasing.Linear.EaseNone;
//					
//				case GoEaseType.BackIn:
//					return GoKitLiteEasing.Back.EaseIn;
//				case GoEaseType.BackOut:
//					return GoKitLiteEasing.Back.EaseOut;
//				case GoEaseType.BackInOut:
//					return GoKitLiteEasing.Back.EaseInOut;
//					
//				case GoEaseType.BounceIn:
//					return GoKitLiteEasing.Bounce.EaseIn;
//				case GoEaseType.BounceOut:
//					return GoKitLiteEasing.Bounce.EaseOut;
//				case GoEaseType.BounceInOut:
//					return GoKitLiteEasing.Bounce.EaseInOut;
//					
//				case GoEaseType.CircIn:
//					return GoKitLiteEasing.Circular.EaseIn;
//				case GoEaseType.CircOut:
//					return GoKitLiteEasing.Circular.EaseOut;
//				case GoEaseType.CircInOut:
//					return GoKitLiteEasing.Circular.EaseInOut;
//					
//				case GoEaseType.CubicIn:
//					return GoKitLiteEasing.Cubic.EaseIn;
//				case GoEaseType.CubicOut:
//					return GoKitLiteEasing.Cubic.EaseOut;
//				case GoEaseType.CubicInOut:
//					return GoKitLiteEasing.Cubic.EaseInOut;
//					
//				case GoEaseType.ElasticIn:
//					return GoKitLiteEasing.Elastic.EaseIn;
//				case GoEaseType.ElasticOut:
//					return GoKitLiteEasing.Elastic.EaseOut;
//				case GoEaseType.ElasticInOut:
//					return GoKitLiteEasing.Elastic.EaseInOut;
//				case GoEaseType.Punch:
//					return GoKitLiteEasing.Elastic.Punch;
//					
//				case GoEaseType.ExpoIn:
//					return GoKitLiteEasing.Exponential.EaseIn;
//				case GoEaseType.ExpoOut:
//					return GoKitLiteEasing.Exponential.EaseOut;
//				case GoEaseType.ExpoInOut:
//					return GoKitLiteEasing.Exponential.EaseInOut;
//					
//				case GoEaseType.QuadIn:
//					return GoKitLiteEasing.Quadratic.EaseIn;
//				case GoEaseType.QuadOut:
//					return GoKitLiteEasing.Quadratic.EaseOut;
//				case GoEaseType.QuadInOut:
//					return GoKitLiteEasing.Quadratic.EaseInOut;
//					
//				case GoEaseType.QuartIn:
//					return GoKitLiteEasing.Quartic.EaseIn;
//				case GoEaseType.QuartOut:
//					return GoKitLiteEasing.Quartic.EaseOut;
//				case GoEaseType.QuartInOut:
//					return GoKitLiteEasing.Quartic.EaseInOut;
//					
//				case GoEaseType.QuintIn:
//					return GoKitLiteEasing.Quintic.EaseIn;
//				case GoEaseType.QuintOut:
//					return GoKitLiteEasing.Quartic.EaseOut;
//				case GoEaseType.QuintInOut:
//					return GoKitLiteEasing.Quartic.EaseInOut;
//					
//				case GoEaseType.SineIn:
//					return GoKitLiteEasing.Sinusoidal.EaseIn;
//				case GoEaseType.SineOut:
//					return GoKitLiteEasing.Sinusoidal.EaseOut;
//				case GoEaseType.SineInOut:
//					return GoKitLiteEasing.Sinusoidal.EaseInOut;
//			}
//			
//			return GoKitLiteEasing.Linear.EaseNone;
		}
	}
}
