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

			var oreAdded = 0;
			var minOre = 50;
			var maxOre = 1000;

			var generator = new MapGenerator (oreToMake, mapSize: 1,
				putOre: (pos, amount) => {
					oreAdded += 1;
					Assert.That (amount,Is.InRange (minOre,maxOre));
				},
				checkAvailable: (pos, amount) => true,
				minOreValue: minOre, maxOreValue: maxOre
			);
				

			Assert.That (oreAdded,Is.EqualTo (generator.oreNum));
		}

	}
}
