using System;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Targeting;
using Server.Gumps;

namespace Server.Items
{
	public class BottleOfAgilityMedium : Item
	{

		[Constructable]
		public BottleOfAgilityMedium() : this( 1 )
		{
		}

		[Constructable]
		public BottleOfAgilityMedium( int amount ) : base( 3854 )
		{
			Weight = 7.0;
			Stackable = true;
			Amount = amount;
			LootType = LootType.Regular;
			Name = "medium bottle of agility";
			Hue = 70;
		}

		public BottleOfAgilityMedium( Serial serial ) : base( serial )
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

		public override void OnDoubleClick( Mobile from )
		{

			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
			else
			{
				from.SendGump( new InternalGump1( from, this ) );
			}
		}

		private class InternalGump1 : Gump
		{

			public InternalGump1( Mobile from, BottleOfAgilityMedium ticket ) : base( 50, 50 )
			{
				Closable=true;
				Disposable=true;
				Dragable=true;
				Resizable=false;
				AddPage(0);
				AddBackground(6, 21, 385, 129, 9200);
				AddHtml( 28, 45, 340, 70, @"This medium bottle is used to enhance your evolution pets dexterity (you need atleast 5 of these to page a GM and have dexterity raised by 1.0 on your evolution pet)",true,true);
			}
		}
	}
}