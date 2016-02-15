using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Xanthos.Evo
{
	public class RaelisDragonEgg : BaseEvoEgg
	{
		public override IEvoCreature GetEvoCreature()
		{
			return new RaelisDragon( "a dragon hatchling" );
		}

		[Constructable]
		public RaelisDragonEgg() : base()
		{
			Name = "a mutant dragon egg";
                        Hue = 1154;
			HatchDuration = 0.01;		// 15 minutes
		}

		public RaelisDragonEgg( Serial serial ) : base ( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}