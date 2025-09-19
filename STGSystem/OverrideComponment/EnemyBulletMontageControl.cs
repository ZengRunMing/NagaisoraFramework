using UnityEngine;

namespace NagaisoraFramework.STGSystem
{
	using static FrameworkMath;

	public class EnemyBulletMontageControl : EnemyBulletControl
	{
		public EnemyBulletControl[] Bullets;

		public Vector2[] BulletVectors;

		public Vector2 Vector;

		public override void Init()
		{
			base.Init();

			if (Bullets is null || Bullets.Length == 0)
			{
				return;
			}

			BulletVectors = new Vector2[Bullets.Length];

			for (int i = 0; i < Bullets.Length; i++)
			{
				Bullets[i].Init();

				BulletVectors[i] = Bullets[i].TransformPosition - TransformPosition;
			}
		}

		public override void Move()
		{
			base.Move();

			float ADSAngle = EulerAngles_ADS(Angle);

			Vector = new Vector2(Sin(ADSAngle), Cos(ADSAngle));

			int i = 0;
			foreach (var item in Bullets)
			{
				item.TransformPosition = (TransformPosition + BulletVectors[i]) * Vector;
				i++;
			}
		}
	}
}