using System;

using UnityEngine;

namespace NagaisoraFramework
{
	public static class FrameworkMath
	{
		public static float[] zsin;
		public static float[] zcos;

		static FrameworkMath()
		{
			zsin = new float[9000];
			zcos = new float[9000];

			for (int i = 0; i < 9000; i++)
			{
				zsin[i] = Mathf.Sin(i * 0.04f * ((float)Math.PI / 180f));
				zcos[i] = Mathf.Cos(i * 0.04f * ((float)Math.PI / 180f));
			}
		}

		#region EulerAnglesMath
		public static float Sin(float In)
		{
			return zsin[(int)(In * 25f)];
		}

		public static float Cos(float In)
		{
			return zcos[(int)(In * 25f)];
		}

		public static float Tan(float In)
		{
			return Sin(In) / Cos(In);
		}

		public static float Distance(Transform a, Transform b)
		{
			float num = Mathf.Pow((a.position.x - b.position.x) * (a.position.x - b.position.x) + (a.position.y - b.position.y) * (a.position.y - b.position.y), 0.5f);
			if (num < 0f)
			{
				num = 0f - num;
			}
			return num;
		}

		public static float EulerAngles_ADS(float a)
		{
			a %= 360f;
			if (a < 0f)
			{
				a += 360f;
			}
			if (a == 360f)
			{
				a = 0f;
			}
			return a;
		}

		public static float EulerAngles_ADS2(float a)
		{
			a %= 360f;
			Debug.Log(a);
			if (a < 0f)
			{
				a += 360f;
			}
			if (a >= 180f)
			{
				a -= 360f;
			}
			if (a == 180f)
			{
				a = -180f;
			}
			return a;
		}

		public static Vector2 GetVectorFromDirection(float Direction)
		{
			return new Vector2(Mathf.Sin(Direction), Mathf.Cos(Direction));
		}
		#endregion

		#region Distances
		public static float Distance(Vector2 p1, Vector2 p2)
		{
			return Mathf.Sqrt((p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y));
		}

		public static float DistanceLine(Vector2 a, Vector2 b, Vector2 c)
		{
			Vector2 vector = b - a;
			Vector2 rhs = c - a;
			float num = Vector2.Dot(vector, rhs);
			if (num < 0f)
			{
				return Distance(c, a);
			}
			float num2 = Vector2.Dot(vector, vector);
			if (num > num2)
			{
				return Distance(c, b);
			}
			num /= num2;
			Vector2 p = a + num * vector;
			return Distance(c, p);
		}

		public static Vector2 Point2lineVerticalPointPosition(Vector2 m, Vector2 a, Vector2 b)
		{
			Vector2 vector = b - a;
			Vector2 rhs = m - a;
			float num = Vector2.Dot(vector, rhs);
			float num2 = Vector2.Dot(vector, vector);
			num /= num2;
			return a + num * vector;
		}
		#endregion

		#region Scale
		public static float Scale(float IN, float IN_Min, float IN_Max, float OUT_Min, float OUT_Max)
		{
			float Std = IN - IN_Min / IN_Max - IN_Min;
			float SAD = Std * (OUT_Max - OUT_Min) + OUT_Min;
			return SAD;
		}

		public static int Scale(int IN, int IN_Min, int IN_Max, int OUT_Min, int OUT_Max)
		{
			int Std = IN - IN_Min / IN_Max - IN_Min;
			int SAD = Std * (OUT_Max - OUT_Min) + OUT_Min;
			return SAD;
		}

		public static long Scale(long IN, long IN_Min, long IN_Max, long OUT_Min, long OUT_Max)
		{
			long Std = IN - IN_Min / IN_Max - IN_Min;
			long SAD = Std * (OUT_Max - OUT_Min) + OUT_Min;
			return SAD;
		}

		public static double Scale(double IN, double IN_Min, double IN_Max, double OUT_Min, double OUT_Max)
		{
			double Std = IN - IN_Min / IN_Max - IN_Min;
			double SAD = Std * (OUT_Max - OUT_Min) + OUT_Min;
			return SAD;
		}
		#endregion
	}
}
