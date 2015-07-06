
using NUnit.Framework;
using UnityEngine;


namespace AssemblyCSharpEditorvs
{
	[TestFixture]
	class AimingControlTests
	{
		[Datapoint] public float x1 = 0, x2 = 1, x3 = 5, x4 = float.MaxValue, x5 = -1, x6 = -10, x7 = 2 * Mathf.PI;

		[Theory]
		public void stayIn2PiTest(float x){
			Assert.That (RotationMath.stayIn2Pi (x),Is.InRange (0f, 2 * Mathf.PI));
		}

		[Test]
		public void horizontalRotationControl(){
			var speed = 1f;
			var angle = 0.1f;
			var direction = 0f;
			var hasTarget = true;
			var targetDir = 1.5f;

			var control = 
				new HorizontalRotationAimingControl (
					rotateSpeed: () => speed,
					fireAngle: () => angle,
					rotateToDirection: d => direction = d,
					hasTarget: () => hasTarget,
					targetDirection: () => targetDir,
					initDirection: targetDir
				);

			Assert.That (direction,Is.EqualTo (targetDir));
			Assert.That (control.ready); 
			control.updateOrientation (0.1f);
			Assert.That (control.ready);

			targetDir = 0.5f;
			for (int i = 0; i < 10; i++) {
				Assert.That (!control.ready);
				control.updateOrientation (0.099f);
			}

			Assert.That (control.ready);
			Assert.That (Mathf.Abs(control.direction-targetDir),Is.LessThan (angle));

		}
	}
}

