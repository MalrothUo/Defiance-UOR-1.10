using System;
using Server;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x2683, 0x2684 )]
	public class ShroudOfIllusions : BaseOuterTorso
	{
		[Constructable]
		public ShroudOfIllusions() : base( 0x2683 )
		{
			Weight = 5.0;
			Name = "Shroud of Illusions";
			Hue = 1175;
			Layer = Layer.OuterTorso;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( Parent != from )
				from.SendMessage( "You must be wearing the robe to use it!" );
			else
			{
				if ( ItemID == 0x2683 || ItemID == 0x2684 )
				{
					ItemID = 0x1F03;
					UnMorph( from );
				}
				else if ( ItemID == 0x1F03 || ItemID == 0x1F04 )
				{
					ItemID = 0x2683;
					Morph( from );
				}
			}
		}

		public override void OnAdded( Object parent )
		{
			base.OnAdded( parent );
			if ( ItemID == 0x2683 && parent is Mobile )
			{
				Mobile from = parent as Mobile;
				Morph( from );
			}
		}

		public override void OnRemoved( Object parent )
		{
			if( parent is Mobile )
			{
				Mobile from = parent as Mobile;
				UnMorph( from );
			}

			base.OnRemoved( parent );
		}

		void Morph( Mobile from )
		{
			from.HueMod = 1;
			from.NameMod = "a shrouded figure";
			from.DisplayGuildTitle = false;
		}

		void UnMorph( Mobile from )
		{
			from.HueMod = -1;
			from.NameMod = null;
			if( from.GuildTitle != null )
				from.DisplayGuildTitle = true;
		}

		public ShroudOfIllusions( Serial serial ) : base( serial )
		{

		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			if ( version < 1 )
				Dyable = false;
		}
	}
}