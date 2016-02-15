using System;
using Server;
using Server.Guilds;

namespace Server.Items
{
	public class HeroKnightShield : BaseShield
	{
		public override int BasePhysicalResistance{ get{ return 1; } }
		public override int BaseFireResistance{ get{ return 0; } }
		public override int BaseColdResistance{ get{ return 0; } }
		public override int BasePoisonResistance{ get{ return 0; } }
		public override int BaseEnergyResistance{ get{ return 0; } }

		public override int InitMinHits{ get{ return 100; } }
		public override int InitMaxHits{ get{ return 125; } }

		public override int AosStrReq{ get{ return 95; } }
		public override int ArmorBase{ get{ return 35; } }

		public override ArmorMeditationAllowance DefMedAllowance{ get{ return ArmorMeditationAllowance.All; } }

		public override bool AllowEquipedCast( Mobile from )
		{
			return true;
		}


		[Constructable]
		public HeroKnightShield() : base( 0x2B01 )
		{
			//if ( !Core.AOS )
			LootType = LootType.Blessed;
			Name = "a hero knight shield";
			Weight = 7.0;
		}

		public HeroKnightShield( Serial serial ) : base(serial)
		{
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( Weight != 7.0 )
				Weight = 7.0;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 );//version
		}




	}
}