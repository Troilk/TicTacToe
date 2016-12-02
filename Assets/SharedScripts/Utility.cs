using UnityEngine;
using System.Collections.Generic;
using Prime31.GoKitLite;

public enum Colors
{
	Red = 0,
	Green,
	Blue
}

static class Utility
{
	#region Const Arrays

	// https://en.wikipedia.org/wiki/Help:Distinguishable_colors
	public static readonly ReadonlyList<Color32> ContrastPalette = new ReadonlyList<Color32>(new Color32[25]{
		new Color32(255, 255, 255, 255),
		new Color32(240, 163, 255, 255),
		new Color32(0, 117, 220, 255),
		new Color32(153, 63, 0, 255),
		new Color32(76, 0, 92, 255),
		new Color32(25, 25, 25, 255),
		new Color32(0, 92, 49, 255),
		new Color32(43, 206, 72, 255),
		new Color32(255, 204, 153, 255),
		new Color32(128, 128, 128, 255),
		new Color32(148, 255, 181, 255),
		new Color32(143, 124, 0, 255),
		new Color32(157, 204, 0, 255),
		new Color32(194, 0, 136, 255),
		new Color32(0, 51, 128, 255),
		new Color32(255, 164, 5, 255),
		new Color32(255, 168, 187, 255),
		new Color32(66, 102, 0, 255),
		new Color32(255, 0, 16, 255),
		new Color32(94, 241, 242, 255),
		new Color32(0, 153, 143, 255),
		new Color32(224, 255, 102, 255),
		new Color32(116, 10, 255, 255),
		new Color32(153, 0, 0, 255),
		//{255,255,128}, // not very contrasting to yellow & Uranium, in my opinion
		//{255,255,0},
		new Color32(255, 80, 5, 255)
	});

	#endregion

	#region Sprite scales
	
	public static void ScaleSpriteForHeight(Camera camera, Sprite sprite, SpriteRenderer spriteRenderer, float heightPercentage)
	{
		float scale = (camera.orthographicSize * 2f * heightPercentage) / sprite.bounds.size.y;
		spriteRenderer.transform.localScale = new Vector3(scale, scale, 1f);
	}
	
	public static void ScaleSpriteForHeight(Camera camera, SpriteRenderer spriteRenderer, float heightPercentage)
	{
		float scale = (camera.orthographicSize * 2f * heightPercentage) / spriteRenderer.sprite.bounds.size.y;
		spriteRenderer.transform.localScale = new Vector3(scale, scale, 1f);
	}
	
	public static Vector3 GetSpriteScaleForHeight(Camera camera, Sprite sprite, float heightPercentage)
	{
		float scale = (camera.orthographicSize * 2f * heightPercentage) / sprite.bounds.size.y;
		return new Vector3(scale, scale, 1f);
	}
	
	public static Vector3 GetSpriteScaleForWidth(Camera camera, Sprite sprite, float widthPercentage)
	{
		float scale = (camera.orthographicSize * 2f * camera.aspect * widthPercentage) / sprite.bounds.size.x;
		return new Vector3(scale, scale, 1f);
	}
	
	public static void GetSpriteScaleForWidth(Camera camera, Sprite sprite, float widthPercentage, out Vector3 scale)
	{
		float scalef = (camera.orthographicSize * 2f * camera.aspect * widthPercentage) / sprite.bounds.size.x;
		scale = new Vector3(scalef, scalef, 1f);
	}
	
	#endregion

	public static Vector4 GetSpriteOffsetAndSize(Sprite sprite)
	{
		Rect texRect = sprite.textureRect;
		Texture tex = sprite.texture;
		int width = tex.width;
		int height = tex.height;

		return new Vector4(texRect.position.x / width,
			texRect.position.y / height,
			texRect.width / width,
			texRect.height / height);
	}

	#region Rect Helpers
	
	public static void RectNormalizeUV(Texture2D tex, ref Rect screenUV)
	{
		float xAspect = 1f / tex.width;
		float yAspect = 1f / tex.height;
		
		screenUV.x *= xAspect;
		screenUV.width *= xAspect;
		screenUV.y *= yAspect;
		screenUV.height *= yAspect;
	}
	
//	public static void RectWorldToScreen(ref Rect worldRect)
//	{
//		CameraBehaviour cam = GameManager.CameraBehaviour;
//		float camWidth = cam.Width, camHeight = cam.Height;
//		
//		float xAspect = Screen.width / camWidth;
//		float yAspect = Screen.height / camHeight;
//		
//		worldRect.x = (worldRect.x + camWidth * 0.5f) * xAspect;
//		worldRect.width *= xAspect;
//		worldRect.y = (camHeight * 0.5f - worldRect.y) * yAspect;
//		worldRect.height *= yAspect;
//	}
	
	public static Rect RectFromCenterSizeScreen(Vector2 center, Vector2 size)
	{
		center -= size * 0.5f;
		return new Rect(center.x, center.y, size.x, size.y);
	}
	
	public static Rect RectFromCenterSizeWorld(Vector2 center, Vector2 size)
	{
		return new Rect(center.x - size.x * 0.5f, center.y + size.y * 0.5f, size.x, size.y);
	}

	#endregion

	#region GameObject and Components Helpers
	
	public static T[] GetOtherComponents<T>(Component[] components) where T: Component
	{
		int componentsCount = components.Length;
		T[] otherComponents = new T[componentsCount];
		
		for(int i = 0; i < componentsCount; ++i)
		{
			otherComponents[i] = components[i].GetComponent<T>();
		}
		
		return otherComponents;
	}

	public static void SetGameObjectsActive(GameObject[] gos, bool active)
	{
		int count = gos.Length;
		for(int i = 0; i < count; ++i)
			gos[i].SetActive(active);
	}

	#endregion

	#region Probabilities helpers

	public static bool ValidateProbabilities(float[] probabilities, bool inPercent, float error, out float total)
	{
		int probCount = probabilities.Length;
		float sum = 0.0f;

		for(int i = 0; i < probCount; ++i)
			sum += probabilities[i];

		total = sum;
		return sum >= 0.0f && Mathf.Abs(sum - error) <= (inPercent ? 100.0f : 1.0f);
	}

	public static float[] NormalizeProbabilities(float[] probabilities)
	{
		int probCount = probabilities.Length;
		float invPerc = 1.0f / 100.0f;

		for(int i = 0; i < probCount; ++i)
			probabilities[i] *= invPerc;

		return probabilities;
	}

	public static float[] ConvertProbabilitiesToDistribution(float[] probabilities)
	{
		int probCount = probabilities.Length;

		for(int i = 1; i < probCount; ++i)
			probabilities[i] += probabilities[i - 1];

		return probabilities;
	}
	
	public static int DiceRoll(float[] probabilitiesDistrib, float[] probabilities)
	{
		int optionsCount = probabilitiesDistrib.Length;
		float diceVal = Random.value;
		
		for(int i = 0; i < optionsCount; ++i)
		{
			if(diceVal <= probabilitiesDistrib[i] && probabilities[i] > 0.0f)
				return i;
		}

		return -1;
	}

	#endregion

	#region Angle Helpers

	/// <summary>
	/// Angle in radians between 2 normalized direction vectors.
	/// </summary>
	/// <returns>Angle</returns>
	/// <param name="fromNormalized">From normalized.</param>
	/// <param name="toNormalized">To normalized.</param>
	public static float AngleNormalized(Vector2 fromNormalized, Vector2 toNormalized)
	{
		return Mathf.Acos(Mathf.Clamp(Vector2.Dot(fromNormalized, toNormalized), -1.0f, 1.0f));
	}

	/// <summary>
	/// Angle in radians between to 2 vectors (taking into account their order)
	/// </summary>
	/// <returns>Signed angle between vectors</returns>
	/// <param name="from">From.</param>
	/// <param name="to">To.</param>
	public static float AngleSigned(Vector2 from, Vector2 to)
	{
		from.Normalize();
		to.Normalize();
		float angle = AngleNormalized(from, to);

		return (from.x * to.y - from.y * to.x) < 0.0f ? -angle : angle;
		//return Vector3.Cross(from, to).z < 0.0f ? -angle : angle;
	}

	/// <summary>
	/// Angle in radians between to 2 vectors (not taking into account their order)
	/// </summary>
	/// <returns>Abs angle between vectors</returns>
	/// <param name="from">From.</param>
	/// <param name="to">To.</param>
	public static float AngleAbs(Vector2 from, Vector2 to)
	{
		from.Normalize();
		to.Normalize();
		return AngleNormalized(from, to);
	}

	/// <summary>
	/// Version of the same method from Mathf class, but works with radians to avoid extra converions
	/// </summary>
	public static float DeltaAngle(float from, float to)
	{
		float num = Mathf.Repeat(to - from, Mathf.PI * 2.0f);
		if (num > Mathf.PI)
		{
			num -= Mathf.PI * 2.0f;
		}
		return num;
	}

	/// <summary>
	/// Version of the same method from Mathf class, but works with radians to avoid extra converions
	/// </summary>
	public static float LerpAngle(float a, float b, float t)
	{
		float num = Mathf.Repeat(b - a, Mathf.PI * 2.0f);
		if (num > Mathf.PI) 
		{
			num -= Mathf.PI * 2.0f;
		}
		return a + num * Mathf.Clamp01(t);
	}

	/// <summary>
	/// Works with radians
	/// </summary>
	public static float LerpAngleUnclamped(float a, float b, float t)
	{
		float num = Mathf.Repeat(b - a, Mathf.PI * 2.0f);
		if (num > Mathf.PI) 
		{
			num -= Mathf.PI * 2.0f;
		}
		return Mathf.Repeat(a + num * t, Mathf.PI * 2.0f);
	}

	#endregion

	#region Random Helpers

	public static Color RandomFromColorRange(Color minColor, Color maxColor)
	{
		minColor.r = Random.Range(minColor.r, maxColor.r);
		minColor.g = Random.Range(minColor.g, maxColor.g);
		minColor.b = Random.Range(minColor.b, maxColor.b);
		minColor.a = Random.Range(minColor.a, maxColor.a);

		return minColor;
	}
	
	public static void RandomOnDonut(ref Vector2 radiusRange)
	{
		float radius = Random.Range(radiusRange.x, radiusRange.y);
		float angle = Random.value * Mathf.PI * 2.0f;
		radiusRange.x = Mathf.Cos(angle) * radius;
		radiusRange.y = Mathf.Sin(angle) * radius;
	}

	public static void RandomOnDonut(ref Vector2 radiusRange, out float angle)
	{
		float radius = Random.Range(radiusRange.x, radiusRange.y);
		angle = Random.value * Mathf.PI * 2.0f;
		radiusRange.x = Mathf.Cos(angle) * radius;
		radiusRange.y = Mathf.Sin(angle) * radius;
	}

	public static void RandomFromSize(ref Vector2 size)
	{
		size.x *= Random.value;
		size.y *= Random.value;
	}

	public static Vector2 RandomOnCircle(float radius)
	{
		float angle = Random.value;
		return new Vector2(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
	}

	public static Vector2 RandomInRectsRange(Rect outer, Rect inner)
	{
		switch(Random.Range(0, 4))
		{
			// top rect
			case 0:
				return new Vector2(Random.Range(outer.xMin, outer.xMax), Random.Range(inner.yMax, outer.yMax));
			// bottom rect
			case 1:
				return new Vector2(Random.Range(outer.xMin, outer.xMax), Random.Range(outer.yMin, inner.yMin));
			// left rect
			case 2:
				return new Vector2(Random.Range(outer.xMin, inner.xMin), Random.Range(inner.yMin, inner.yMax));
			// right rect
			case 3:
				return new Vector2(Random.Range(inner.xMax, outer.xMax), Random.Range(inner.yMin, inner.yMax));
		}
			
		Debug.LogError("Utility.RandomInRectsRange(): Part of the code should not be reached", null);
		return Vector2.zero;
	}

	/// <summary>
	/// !This function does not get uniform distribution in area
	/// </summary>
	/// <returns>Random point between inner circle and outer rect</returns>
	/// <param name="outer">half size of outer rect</param>
	/// <param name="innerRad">radius of inner circle</param>
	public static Vector2 RandomInRectAndCircleRange(Vector2 outer, float innerRad)
	{
		float yVal = Random.Range(-outer.y, outer.y);
		outer.y = yVal;

		if(yVal <= -innerRad || yVal >= innerRad)
			// out of circle horizontal range
			outer.x = Random.Range(-outer.x, outer.x);
		else
			outer.x = Random.Range(Mathf.Sqrt(Mathf.Max(0.0f, innerRad * innerRad - yVal * yVal)), outer.x) * (Random.value < 0.5f ? -1.0f : 1.0f);

		return outer;
	}

	public static Vector2 RandomInDonutSegment(Vector2 center, Vector2 radiusRange, Vector2 angleRange)
	{
		float radius = Random.Range(radiusRange.x, radiusRange.y);
		float angle = Random.Range(angleRange.x, angleRange.y);

		center.x += Mathf.Cos(angle) * radius;
		center.y += Mathf.Sin(angle) * radius;

		return center;
	}

	public static void SelectRandomMfromN<T>(T[] values, T[] resultsPool, int valuesCount, int count)
	{
		int idx;
		T temp;

		for(int i = 0; i < count; ++i)
		{
			idx = Random.Range(0, valuesCount - i);
			temp = values[idx];
			resultsPool[i] = temp;

			// swap - move selected to end
			values[idx] = values[(valuesCount - i) - 1];
			values[(valuesCount - i) - 1] = temp;
		}
	}

	public static void SelectRandomMfromN<T>(List<T> values, T[] resultsPool, int count)
	{
		int idx;
		
		for(int i = 0; i < count; ++i)
		{
			idx = Random.Range(0, values.Count);
			resultsPool[i] = values[idx];
			values.RemoveAt(idx);
		}
	}

	public static void RandomizeOrder<T>(T[] values)
	{
		int count = values.Length;

		for(int i = 0; i < count - 1; ++i)
		{
			int idx = Random.Range(i + 1, count);
			T temp = values[idx];
			values[idx] = values[i];
			values[i] = temp;
		}
	}

	#endregion

	#region Points in shapes

	public static bool PointInDonut(Vector2 point, Vector2 donutCenter, Vector2 donutSqrRadiusRange)
	{
		point.x -= donutCenter.x;
		point.y -= donutCenter.y;
		float sqrMag = point.sqrMagnitude;
		return (sqrMag >= donutSqrRadiusRange.x && sqrMag <= donutSqrRadiusRange.y);
	}
	
	public static bool PointInCone(ref Vector2 point, ref Vector2 coneCenter, ref Vector2 coneCenterDirectionNormalized,
	                               float radius, float halfAngle)
	{
		Vector2 directionToPoint = point - coneCenter;
		float distanceToPoint = directionToPoint.magnitude;
		
		if(distanceToPoint > radius)
			return false;
		
		directionToPoint *= (1.0f / distanceToPoint);
		return Mathf.Abs(Mathf.Acos(directionToPoint.x * coneCenterDirectionNormalized.x + directionToPoint.y * coneCenterDirectionNormalized.y)) < halfAngle;
	}
	
	public static bool PointInConeOpt(ref Vector2 point, ref Vector2 coneCenter, ref Vector2 coneCenterDirectionNormalized,
	                                  float radius, float halfAngleCos)
	{
		Vector2 directionToPoint = point - coneCenter;
		float distanceToPoint = directionToPoint.magnitude;
		
		if(distanceToPoint > radius)
			return false;
		
		directionToPoint *= (1.0f / distanceToPoint);
		return (directionToPoint.x * coneCenterDirectionNormalized.x + directionToPoint.y * coneCenterDirectionNormalized.y) > halfAngleCos;
	}

	#endregion

	public static bool BoxOverlapsAnything(Vector2 boxCenter, Vector2 boxSize, Vector2 safeOffset)
	{
		boxSize.x = boxSize.x * 0.5f + safeOffset.x;
		boxSize.y = boxSize.y * 0.5f + safeOffset.y;
		return Physics2D.OverlapArea(boxCenter - boxSize, boxCenter + boxSize) != null;
	}
	
	public static bool ValidateArrayNotNull<T>(T[] arr, int expectedLength)
	{
		int l = arr.Length;
		if(l != expectedLength)
			return false;
		
		for(int i = 0; i < l; ++i)
			if(arr[i] == null)
				return false;
		
		return true;
	}

	public static void FillArrayWithIncreasingValues(int[] arr, int start, int step)
	{
		int count = arr.Length;
		start -= step;
		for(int i = 0; i < count; ++i)
			arr[i] = start += step;
	}

	public static float[] GetIncrementingArrayWithBaseValue(float baseValue, float[] increments)
	{
		int incCount = increments.Length;
		float[] result = new float[incCount + 1];
		result[0] = baseValue;

		for(int i = 0; i < incCount; ++i)
			result[i + 1] = result[i] + increments[i];

		return result;
	}

	public static int GetHammingWeight(int value)
	{
		value = value - ((value >> 1) & 0x55555555);
		value = (value & 0x33333333) + ((value >> 2) & 0x33333333);
		return (((value + (value >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24;
	}

	public static int RoundToInt(double value)
	{
		return value > 0.0 ? (int)(value + 0.5) :
							 (int)(value - 0.5);
	}

	public static int RoundToInt(float value)
	{
		return value > 0.0f ? (int)(value + 0.5f) :
			(int)(value - 0.5f);
	}

	public static long RoundToLong(double value)
	{
		return value > 0.0 ? (long)(value + 0.5) :
			(long)(value - 0.5);
	}

	public static void FloorVector(ref Vector3 vec)
	{
		vec.x = Mathf.Floor(vec.x);
		vec.y = Mathf.Floor(vec.y);
		vec.z = Mathf.Floor(vec.z);
	}

	public static float AngleFromChord(float radius, float chordLength)
	{
		return Mathf.Asin(chordLength / (radius * 2.0f)) * 2.0f;
	}

	public static float ChordFromAngle(float radius, float angle)
	{
		return 2.0f * radius * Mathf.Sin(angle * 0.5f);
	}

	public static float DistanceToChord(float radius, float chordLength)
	{
		return Mathf.Sqrt(radius * radius - chordLength * chordLength * 0.25f);
	}

	public static Rect[] OverlappingRectsTo4RectsClockwise(Rect inner, Rect outer)
	{
		Rect[] rects = new Rect[4];

		rects[0] = Rect.MinMaxRect(outer.xMin, inner.yMax, outer.xMax, outer.yMax);
		rects[1] = Rect.MinMaxRect(inner.xMax, inner.yMin, outer.xMax, inner.yMax);
		rects[2] = Rect.MinMaxRect(outer.xMin, outer.yMin, outer.xMax, inner.yMin);
		rects[3] = Rect.MinMaxRect(outer.xMin, inner.yMin, inner.xMin, inner.yMax);

		return rects;
	}

	#region Mesh helpers

	public static Mesh GenerateQuadMesh(float width, float height,
	                                    bool useUVs = false,
	                                    Vector2 uvXRange = default(Vector2), Vector2 uvYRange = default(Vector2))
	{
		Mesh m = new Mesh();
		m.name = "QuadMesh";

		float hw = width * 0.5f;
		float hh = height * 0.5f;
		
		m.vertices = new Vector3[] {
			new Vector3(-hw, -hh, 0.0f),
			new Vector3(hw, -hh, 0.0f),
			new Vector3(hw, hh, 0.0f),
			new Vector3(-hw, hh, 0.0f)
		};

		if(useUVs)
		{
			m.uv = new Vector2[] {
				new Vector2(uvXRange.x, uvYRange.x),
				new Vector2(uvXRange.y, uvYRange.x),
				new Vector2(uvXRange.y, uvYRange.y),
				new Vector2(uvXRange.x, uvYRange.y)
			};
		}
		
		m.triangles = new int[] { 0, 1, 2, 0, 2, 3};
		return m;
	}

	/// <summary>
	/// Generates the mesh and prepares renderer on specified gameobject for rendering it
	/// </summary>
	/// <returns>Material from <c>MeshRenderer</c> component on specified gameobject</returns>
	/// <param name="go">Gameobject with <c>MeshFilter</c> and <c>MeshRenderer</c> components</param>
	/// <param name="width">Width of mesh (2.0 - fullscreen width)</param>
	/// <param name="height">Height of mesh (2.0 - fullscreen height)</param>
	/// <param name="useUVs">If set to <c>true</c> generate UVs</param>
	/// <param name="uvXRange">Range of UVs on <c>X</c> axis. Example: [0.0, 1.0]</param>
	/// <param name="uvYRange">Range of UVs on <c>Y</c> axis. Example: [0.0, 1.0]</param>
	public static Material GenerateMeshAndPrepareRendererForQuadShaderEffect(GameObject go,
	                                                                         float width, float height,
	                                                                         bool useUVs = false,
	                                                                         Vector2 uvXRange = default(Vector2), Vector2 uvYRange = default(Vector2))
	{
		// creating fullscreen quad
		Mesh quad = Utility.GenerateQuadMesh(width, height,
		                                     useUVs,
		                                     uvXRange, uvYRange);
		
		return PrepareRendererForEffect(go, quad, false);
	}

	public static Material PrepareRendererForEffect(GameObject go, Mesh mesh, bool useSharedMesh)
	{
		// setting up mesh filter
		if(useSharedMesh)
			go.GetComponent<MeshFilter>().sharedMesh = mesh;
		else
			go.GetComponent<MeshFilter>().mesh = mesh;

		// setting up mesh renderer
		MeshRenderer mr = go.GetComponent<MeshRenderer>();
		mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		mr.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
		mr.receiveShadows = false;
		mr.useLightProbes = false;

		// getting effect material
		return mr.material;
	}

	public static Mesh GenerateDonutMesh(int radialSegments, float radiusInner, float radiusOuter, bool generateUVs)
	{
		int vertsCount = radialSegments * 2;
		Vector3[] verts = new Vector3[vertsCount];
		int[] indices = new int[radialSegments * 6];
		Vector2[] uvs = null;
		if(generateUVs)
			uvs = new Vector2[radialSegments * 2];

		int vertIdx = -1, idxIdx = -1;
		float angleMult = Mathf.PI * 2.0f / radialSegments;
		float angle, angleSin, angleCos;
		float innerRadFraction = radiusInner / radiusOuter;

		for(int i = 0; i < radialSegments; ++i)
		{
			angle = i * angleMult;
			angleSin = Mathf.Sin(angle);
			angleCos = Mathf.Cos(angle);

			vertIdx = i * 2;

			verts[vertIdx] = new Vector3(angleCos * radiusInner, angleSin * radiusInner);
			verts[vertIdx + 1] = new Vector3(angleCos * radiusOuter, angleSin * radiusOuter);

			if(generateUVs)
			{
				// transform into [0, 1] range
				angleSin = angleSin * 0.5f + 0.5f;
				angleCos = angleCos * 0.5f + 0.5f;

				uvs[vertIdx] = new Vector2(angleCos * innerRadFraction, angleSin * innerRadFraction);
				uvs[vertIdx + 1] = new Vector2(angleCos, angleSin);
			}

			indices[++idxIdx] = vertIdx + 1;
			indices[++idxIdx] = vertIdx;
			indices[++idxIdx] = (vertIdx + 2) % vertsCount;

			indices[++idxIdx] = vertIdx + 1;
			indices[++idxIdx] = (vertIdx + 2) % vertsCount;
			indices[++idxIdx] = (vertIdx + 3) % vertsCount;
		}

		// create mesh
		Mesh mesh = new Mesh();
		mesh.vertices = verts;
		mesh.triangles = indices;
		if(generateUVs)
			mesh.uv = uvs;

		return mesh;
	}

	public static Mesh GenerateCircleMinPolygonsFromFirst(int radialSegments, float radius, bool generateUVs)
	{
		Vector3[] verts = new Vector3[radialSegments];
		int[] tris = new int[(radialSegments - 2) * 3];
		Vector2[] uvs = null;
		if(generateUVs)
			uvs = new Vector2[radialSegments];

		float angleMult = Mathf.PI * 2.0f / radialSegments;
		float angle, angleSin, angleCos;

		for(int i = 0; i < radialSegments; ++i)
		{
			angle = i * angleMult;
			angleSin = Mathf.Sin(angle);
			angleCos = Mathf.Cos(angle);

			verts[i] = new Vector3(angleCos * radius, angleSin * radius);

			if(generateUVs)
			{
				// transform into [0, 1] range
				angleSin = angleSin * 0.5f + 0.5f;
				angleCos = angleCos * 0.5f + 0.5f;

				uvs[i] = new Vector2(angleCos, angleSin);
			}
		}

		// generating triangles
		radialSegments -= 2;
		int trisIdx = -1;

		for(int i = 0; i < radialSegments; ++i)
		{
			tris[++trisIdx] = 0;
			tris[++trisIdx] = i + 2;
			tris[++trisIdx] = i + 1;
		}

		// create mesh
		Mesh mesh = new Mesh();
		mesh.vertices = verts;
		mesh.triangles = tris;
		if(generateUVs)
			mesh.uv = uvs;

		return mesh;
	}

	#endregion

	#region Renderer Helpers

//	static Mesh fullScreenQuad;
//	public static void Blit(RenderTexture source, RenderTexture destination, Material material, int pass)
//	{
//		if(fullScreenQuad == null)
//		{
//			fullScreenQuad = GenerateQuadMesh(1.0f, 1.0f,
//			                                  true,
//			                                  new Vector2(0.0f, 1.0f), new Vector2(0.0f, 1.0f));
//			fullScreenQuad.UploadMeshData(true);
//		}
//		
//		Graphics.SetRenderTarget(destination);
//		material.mainTexture = source;
//		material.SetPass(pass);
//
//		float height = Camera.main.orthographicSize * 2.0f;
//		float width = height * Camera.main.aspect;
//		Graphics.DrawMeshNow(fullScreenQuad, Matrix4x4.Scale(new Vector3(width, height, 1.0f)));
//	}

	#endregion

	// Extension methods

	#region UnityEngine.Object extensions

	public static bool IsRealNull(this UnityEngine.Object aObj)
	{
		return (object)aObj == null;
	}

	#endregion

	#region Color extensions

	public static int ToHex(this Color32 color)
	{
		return color.r << 24 | color.g << 16 | color.b << 8 | color.a;
	}

	public static string ToHexStr(this Color32 color)
	{
		return color.ToHex().ToString("X8");
	}

	public static Color32 ToRGB(this int hexColor)
	{
		Color32 col = new Color32();
		col.r = (byte)((hexColor >> 24) & 0xFF);
		col.g = (byte)((hexColor >> 16) & 0xFF);
		col.b = (byte)((hexColor >> 8) & 0xFF);
		col.a = (byte)(hexColor & 0xFF);

		return col;
	}

	#endregion

	#region Rect extensions

	public static void DrawGizmo(this Rect rect)
	{
		Vector3 leftBottom = new Vector3(rect.xMin, rect.yMin);
		Vector3 leftTop = new Vector3(rect.xMin, rect.yMax);
		Vector3 rightTop = new Vector3(rect.xMax, rect.yMax);
		Vector3 rightBottom = new Vector3(rect.xMax, rect.yMin);

		Gizmos.DrawLine(leftBottom, leftTop);
		Gizmos.DrawLine(leftTop, rightTop);
		Gizmos.DrawLine(rightTop, rightBottom);
		Gizmos.DrawLine(rightBottom, leftBottom);
	}

	public static Rect Offset(this Rect rect, float offset)
	{
		return new Rect(rect.x - offset, rect.y - offset, rect.width + offset * 2.0f, rect.height + offset * 2.0f);
	}

	public static Vector4 ToVector4(this Rect rect)
	{
		return new Vector4(rect.x, rect.y, rect.width, rect.height);
	}

	#endregion

	#region RectTransform extensions

	public static void ScretchToParent(this RectTransform transf)
	{
		transf.anchorMin = Vector2.zero;
		transf.anchorMax = Vector2.one;
		transf.offsetMin = Vector2.zero;
		transf.offsetMax = Vector2.zero;
	}

	#endregion
	
	#region GameObject extensions
	/// <summary>
	/// WARNING! Generates garbage - use only in initialization.
	/// </summary>
	/// <returns>The size based on all sprites.</returns>
	/// <param name="go">game object</param>
	public static Vector2 CalculateSizeBasedOnAllSprites(this GameObject go)
	{
		Bounds sizeBounds = new Bounds();
		SpriteRenderer[] spriteRenderers = go.GetComponentsInChildren<SpriteRenderer>();
		Sprite sprite;
		
		for(int i = 0; i < spriteRenderers.Length; ++i)
		{
			sprite = spriteRenderers[i].sprite;
			if(sprite != null)
				sizeBounds.Encapsulate(sprite.bounds);
		}
		
		return sizeBounds.size;
	}
	
	public static T GetComponentInCurrentOrParent<T>(this GameObject go) where T: Component
	{
		T component = go.GetComponent<T>();
		if(component == null)
		{
			Transform parentTr = go.transform.parent;
			if(parentTr != null)
				component = parentTr.GetComponent<T>();
		}
		
		return component;
	}
	
	public static GameObject[] GetChildGameObjects(this GameObject parentObject)
	{
		Transform parentTransform = parentObject.transform;
		int childCount = parentTransform.childCount;
		GameObject[] childObjects = new GameObject[childCount];
		
		for(int i = 0; i < childCount; ++i)
		{
			childObjects[i] = parentTransform.GetChild(i).gameObject;
		}
		
		return childObjects;
	}

	public static List<GameObject> GetDirectChildrenWithComponentInChildren<T>(this GameObject parentObject) where T: Component
	{
		Transform parentTransform = parentObject.transform;
		int childCount = parentTransform.childCount;
		List<GameObject> childObjects = new List<GameObject>();

		for(int i = 0; i < childCount; ++i)
		{
			Transform child = parentTransform.GetChild(i);
			if(child.GetComponentInChildren<T>(true) != null)
				childObjects.Add(child.gameObject);
		}

		return childObjects;
	}

	public static T GetComponentInDirectChildren<T>(this GameObject parentObject) where T: Component
	{
		Transform parentTransform = parentObject.transform;
		int childCount = parentTransform.childCount; 
		T result = null;

		for(int i = 0; i < childCount; ++i)
		{
			if((result = parentTransform.GetChild(i).GetComponent<T>()) != null)
				return result;
		}

		return result;
	}
	
	public static T[] GetComponentsInDirectChildren<T>(this GameObject parentObject) where T: Component
	{
		GameObject[] childObjects = parentObject.GetChildGameObjects();
		int childCount = childObjects.Length;
		List<T> components = new List<T>();
		T component;
		
		for(int i = 0; i < childCount; ++i)
		{
			component = childObjects[i].GetComponent<T>();
			if(component != null)
				components.Add(component);
		}
		
		return components.ToArray();
	}

	/// <summary>
	/// Searches on inactive parents too
	/// </summary>
	public static T GetComponentInParents<T>(this GameObject go) where T: Component
	{
		Transform transf = go.transform;
		T val;

		while((transf = transf.parent) != null)
		{
			if((val = transf.GetComponent<T>()) != null)
				return val;
		}
		return null;
	}

	public static void DestroyAllChildrenExcept(this GameObject parentObject, GameObject exceptionObject)
	{
		Transform goTr = parentObject.transform;
		GameObject child;
		int childId = goTr.childCount - 1;
		
		while(childId >= 0)
		{
			child = goTr.GetChild(childId).gameObject;
			if(child != exceptionObject)
				GameObject.Destroy(child);
			--childId;
		}
	}
	
	public static void DestroyAllChildren(this GameObject parentObject)
	{
		Transform goTr = parentObject.transform;
		int childId = goTr.childCount - 1;
		
		while(childId >= 0)
		{
			GameObject.Destroy(goTr.GetChild(childId).gameObject);
			--childId;
		}
	}

	public static void DestroyComponentsDerivedFrom<T>(this GameObject go) where T: Component
	{
		foreach(T comp in go.GetComponents<T>())
			Component.Destroy(comp);
	}
	
	#endregion

	#region Vector2 extensions

	public static Vector2 Abs(this Vector2 vec)
	{
		vec.x = Mathf.Abs(vec.x);
		vec.y = Mathf.Abs(vec.y);
		return vec;
	}
	
	public static Vector2 Rotate(this Vector2 vec, float angle)
	{
		float sin = Mathf.Sin(angle);
		float cos = Mathf.Cos(angle);
		
		return new Vector2(vec.x * cos - vec.y * sin, vec.x * sin + vec.y * cos);
	}
	
	public static Vector2 ReflectNormalized(this Vector2 vec, Vector2 normalizedNormal)
	{
		float val = 2.0f * ((vec.x * normalizedNormal.x) + (vec.y * normalizedNormal.y));
		vec.x -= normalizedNormal.x * val;
		vec.y -= normalizedNormal.y * val;
		return vec;
	}

	public static Vector2 GetRandomPerpendicular(this Vector2 vec)
	{
		if(Random.value < 0.5f)
			return new Vector2(vec.y, -vec.x);
		else
			return new Vector2(-vec.y, vec.x);
	}
	
	public static float DistanceToLine(this Vector2 point, Vector2 lineStart, Vector2 lineEnd)
	{
		Vector2 dir1 = lineEnd - lineStart;
		Vector2 dir2 = point - lineStart;
		
		float c1 = dir1.x * dir2.x + dir1.y * dir2.y;
		if(c1 <= 0.0f)
			return Vector2.Distance(point, lineStart);
		
		float c2 = dir1.x * dir1.x + dir1.y + dir1.y;
		if(c2 <= c1)
			return Vector2.Distance(point, lineEnd);
		
		float b = c1 / c2;
		return Vector2.Distance(point, lineStart + b * dir1);
	}

	public static float Lerp(this Vector2 range, float t)
	{
		return Mathf.Lerp(range.x, range.y, t);
	}

	public static float LerpUnclamped(this Vector2 range, float t)
	{
		return Mathf.LerpUnclamped(range.x, range.y, t);
	}

	public static float LerpAngle(this Vector2 range, float t)
	{
		return Utility.LerpAngle(range.x, range.y, t);
	}

	public static float LerpAngleUnclamped(this Vector2 range, float t)
	{
		return Utility.LerpAngleUnclamped(range.x, range.y, t);
	}

	public static bool CheckInBounds(this Vector2 vec, Vector2 horizontalBounds, Vector2 verticalBounds)
	{
		return (vec.x > horizontalBounds.x &&
		        vec.x < horizontalBounds.y &&
		        vec.y > verticalBounds.x &&
		        vec.y < verticalBounds.y);
	}

	public static float Max(this Vector2 vec)
	{
		return vec.x > vec.y ? vec.x : vec.y;
	}
	
	#endregion
	
	#region Random related extensions
	
	public static float RandomFromRange(this Vector2 rangeVec)
	{
		return Random.Range(rangeVec.x, rangeVec.y);
	}
	
	public static Vector2 RandomOnDonut(this Vector2 radiusRange)
	{
		float rad = Random.Range(radiusRange.x, radiusRange.y);
		float angle = Random.value * Mathf.PI * 2.0f;
		return new Vector2(Mathf.Cos(angle) * rad, Mathf.Sin(angle) * rad);
	}

	public static Vector2 GetRandomPoint(this Rect rect)
	{
		return new Vector2(Random.Range(rect.xMin, rect.xMax), Random.Range(rect.yMin, rect.yMax));
	}
	
	#endregion

	#region Libs extensions

	public static void CheckStopTween(ref int tweenId, bool bringToCompletion)
	{
		if(tweenId != -1)
		{
			GoKitLite.instance.stopTween(tweenId, bringToCompletion);
			tweenId = -1;
		}
	}

	#endregion
}
