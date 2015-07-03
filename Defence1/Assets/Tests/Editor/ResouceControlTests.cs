using System;
using UnityEngine;
using NUnit.Framework;


namespace AssemblyCSharpEditorvs
{
	[TestFixture]
	public class ResouceControlTests
	{
		[Test]
		public void tryChangeOreTests(){
			var invokedUpdate = false;
			var control = new ResourceControl (50,v=> invokedUpdate = true);

			Assert.That (control.tryChangeOre (0));
			Assert.That (control.ore,Is.EqualTo (50));
			Assert.That (invokedUpdate);

			Assert.That (control.tryChangeOre (50));
			Assert.That (control.ore,Is.EqualTo(100));

			Assert.That (control.tryChangeOre (-150) == false);
			Assert.That (control.ore,Is.EqualTo (100));
		}
	}
}

