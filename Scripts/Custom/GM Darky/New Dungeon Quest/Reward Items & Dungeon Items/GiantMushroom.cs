using System;

namespace Server.Items
{
	public class GiantMushroom: Item
	{
		public override string DefaultName{ get{ return "a mushroom"; } }

		[Constructable]
		public GiantMushroom() : base( Utility.Random( 0x222E, 4 ) )
		{
			Weight = 9.0;
		}

		public GiantMushroom(Serial serial) : base(serial)
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize(writer);

			writer.WriteEncodedInt( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}

	}
}