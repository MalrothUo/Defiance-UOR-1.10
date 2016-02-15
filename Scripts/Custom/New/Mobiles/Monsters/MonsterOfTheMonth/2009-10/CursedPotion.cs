using System;
using Server;

namespace Server.Items
{
	public class CursedPotion : Item
	{
		public override string DefaultName{ get{ return "a cursed potion"; } }

		[Constructable]
		public CursedPotion() : this( Utility.RandomList( 3835, 3836, 3837, 3842 ) )
		{
		}

		[Constructable]
		public CursedPotion( int itemID ) : base( itemID )
		{
			Weight = 1;
		}

		public CursedPotion( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}