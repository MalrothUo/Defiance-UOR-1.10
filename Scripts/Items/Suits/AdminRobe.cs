using System;
using Server;

namespace Server.Items
{
	public class AdminRobe : BaseSuit
	{
		public override string DefaultName{ get{ return "Admin Robe"; } }

		[Constructable]
		public AdminRobe() : base( AccessLevel.Administrator, 1150, 0x204F ) // Blank hue
		{
		}

		public AdminRobe( Serial serial ) : base( serial )
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