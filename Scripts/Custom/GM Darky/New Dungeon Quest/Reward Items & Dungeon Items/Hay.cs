using System;

namespace Server.Items
{
	public class Hay : Item
	{
		public override string DefaultName{ get{ return "hay"; } }

		[Constructable]
		public Hay() : base( Utility.RandomBool() ? 0xF34 : 0xF35 )
		{
			Weight = 9.0;
		}

		public Hay(Serial serial) : base(serial)
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