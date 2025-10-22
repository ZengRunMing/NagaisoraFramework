using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace NagaisoraFramework.STGSystem
{
	public static class GenericFunction
	{
		public enum AlignmentMode
		{
			Left, Middle, Right
		}

		public static (GameObject, T)[] NewCircularBullet<T>(string identifier, int order, int count, int type, int color, Vector2 position, float initialAngle, Assembly ECLAssembly, STGControler controler) where T : EnemyBulletControl
		{
			initialAngle = FrameworkMath.EulerAngles_ADS(initialAngle);

			float Angle = initialAngle;
			float AngleStep = 360f / count;

			List<(GameObject, T)> results = new List<(GameObject, T)>();

			controler.NewEnemyShootEffect<EnemyShootEffectControl>(color, 0, position, true, BlendMode.AlphaBlend);

			for (int i = 0; i < count; i++)
			{
				(GameObject obj, T Componment) = controler.NewEnemyBullet<T>(type, color, $"CircularBullet_{identifier}_{i}", order, position, Angle, false);

				Componment.Init(ECLAssembly);
				Componment.ECLControler?.Run();

				results.Add((obj, Componment));

				Angle += AngleStep;
			}

			return results.ToArray();
		}

		public static (GameObject, T)[] NewPolygonalRingBullet<T>(string identifier, int order, int count, int type, int color, Vector2 position, float initialAngle, Assembly ECLAssembly, STGControler controler) where T : EnemyBulletControl
		{
			initialAngle = FrameworkMath.EulerAngles_ADS(initialAngle);

			float Angle = initialAngle;
			float AngleStep = 360f / count;

			List<(GameObject, T)> results = new List<(GameObject, T)>();

			controler.NewEnemyShootEffect<EnemyShootEffectControl>(color, 0, position, true, BlendMode.AlphaBlend);

			for (int i = 0; i < count; i++)
			{
				(GameObject obj, T Componment) = controler.NewEnemyBullet<T>(type, color, $"PolygonalRingBullet_{identifier}_{i}", order, position, Angle, false);

				Componment.Init(ECLAssembly);
				Componment.ECLControler?.Run();

				results.Add((obj, Componment));

				Angle += AngleStep;
			}

			return results.ToArray();
		}

		public static (GameObject, T)[] NewSectorBullet<T>(string identifier, int order, int count, int type, int color, Vector2 position, float unilateralAngle, float initialAngle, AlignmentMode mode, Assembly ECLAssembly, STGControler controler) where T : EnemyBulletControl
		{
			initialAngle = FrameworkMath.EulerAngles_ADS(initialAngle);

			float Angle;
			float AngleStep = unilateralAngle / (count - 1);

			if (mode == AlignmentMode.Middle)
			{
				Angle = initialAngle + (unilateralAngle / 2f);
			}
			else
			{
				Angle = initialAngle;
			}

			if (mode == AlignmentMode.Left || mode == AlignmentMode.Middle)
			{
				AngleStep = -AngleStep;
			}

			List<(GameObject, T)> results = new List<(GameObject, T)>();

			controler.NewEnemyShootEffect<EnemyShootEffectControl>(color, 0, position, true, BlendMode.AlphaBlend);

			for (int i = 0; i < count; i++)
			{
				(GameObject obj, T Componment) = controler.NewEnemyBullet<T>(type, color, $"SectorBullet_{identifier}_{i}", order, position, Angle, false);
				
				Componment.Init(ECLAssembly);
				Componment.ECLControler?.Run();

				results.Add((obj, Componment));

				Angle += AngleStep;
			}

			return results.ToArray();
		}

		public static (GameObject, T) NewRandomDirectionBullet<T>(string identifier, int order, int type, int color, Vector2 position, float angleMin, float angleMax, Assembly ECLAssembly, STGControler controler) where T : EnemyBulletControl
		{
			float angle = MainSystem.RandomFloat(angleMin, angleMax);

			controler.NewEnemyShootEffect<EnemyShootEffectControl>(color, 0, position, true, BlendMode.AlphaBlend);
			
			(GameObject obj, T Componment) = controler.NewEnemyBullet<T>(type, color, $"RandomDirectionBullet_{identifier}", order, position, angle, false);
			
			Componment.Init(ECLAssembly);
			Componment.ECLControler?.Run();
			
			return (obj, Componment);
		}

		public static (GameObject, T) NewAimedBullet<T>(string identifier, int order, int type, int color, Vector2 position, Vector2 targetPosition, float initialAngle, Assembly ECLAssembly, STGControler controler) where T : EnemyBulletControl
		{
			initialAngle = FrameworkMath.EulerAngles_ADS(initialAngle);

			Vector2 direction = targetPosition - position;

			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + initialAngle;
			
			controler.NewEnemyShootEffect<EnemyShootEffectControl>(color, 0, position, true, BlendMode.AlphaBlend);
			
			(GameObject obj, T Componment) = controler.NewEnemyBullet<T>(type, color, $"AimedBullet_{identifier}", order, position, angle, false);
			
			Componment.Init(ECLAssembly);
			Componment.ECLControler?.Run();
			
			return (obj, Componment);
		}

		public static T[] RandomArrayArrangement<T>(T[] i)
		{
			List<T> source = new List<T>(i);

			List<T> result = new List<T>();

			while (source.Count > 0)
			{
				int index = MainSystem.RandomInt(0, source.Count - 1);
				result.Add(source[index]);
				source.RemoveAt(index);
			}

			return result.ToArray();
		}
		
		public static float[] GetAngleDivisionArray(int count, float Angle)
		{
			float[] result = new float[count];

			float AngleStep = Angle / count;

			float currentAngle = 0f;

			for (int i = 0; i < count; i++)
			{
				result[i] = currentAngle;
				currentAngle += AngleStep;
			}

			return result;
		}

		//public static 
	}
}
