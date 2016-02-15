using System;

namespace Server.Items
{
	public class FrozenBook : Item
	{
		public override string DefaultName{ get{ return "Book of the Frozen Tear"; } }

		[Constructable]
		public FrozenBook() : base(0xEFA)
		{
			Weight = 9.0;
			Hue = 1072;
			LootType = LootType.Newbied;
			Layer = Layer.FirstValid;
		}

		public FrozenBook(Serial serial) : base(serial)
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