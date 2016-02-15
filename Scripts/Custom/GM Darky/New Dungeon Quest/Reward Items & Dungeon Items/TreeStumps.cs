using System;

namespace Server.Items
{
	public class TreeStumps : Item
	{
		public override string DefaultName{ get{ return "a stump"; } }

		[Constructable]
		public TreeStumps() : base( Utility.Random( 0xE56, 4 ) )
		{
			Weight = 9.0;
		}

		public TreeStumps(Serial serial) : base(serial)
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