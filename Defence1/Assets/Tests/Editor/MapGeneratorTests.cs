using System;
using UnityEngine;
using NUnit.Framework;

namespace AssemblyCSharpEditorvs
{
	[TestFixture]
	internal class MapGeneratorTests
	{

		[Datapoint] public int zero = 0;
		[Datapoint] public int less = 10;
		[Datapoint] public int many = 1000;

		[Theory]
		public void OreGenerationTest(int oreToMake){
			Assume.That (oreToMake>=0 && oreToMake < 5000);

			var generator = new MapGenerator (oreToMake);

			var oreAdded = 0;
			generator.generateOres (
				genFunc: (pos,amount)=>{
					oreAdded += 1;
					Assert.That (amount,Is.InRange (generator.minOreValue,generator.maxOreValue));
				},
				randomPosInScene: a =>Vector2.zero
			);

			Assert.That (oreAdded,Is.EqualTo (generator.oreNum));
		}

	}
}
