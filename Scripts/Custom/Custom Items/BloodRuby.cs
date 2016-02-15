using System;
using Server;

namespace Server.Items
{
	public class BloodRuby : Item
	{
		[Constructable]
		public BloodRuby() : this( 1 )
		{
		}

		[Constructable]
		public BloodRuby( int amount ) : base( 0xF13 )
		{
			Stackable = false;
			Weight = 30;
			Amount = amount;
		}

		public BloodRuby( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}