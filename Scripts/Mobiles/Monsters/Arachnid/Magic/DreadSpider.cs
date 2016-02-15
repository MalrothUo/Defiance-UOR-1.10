using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a dread spider corpse" )]
	public class DreadSpider : BaseCreature
	{
		public override string DefaultName{ get{ return "a dread spider"; } }

		[Constructable]
		public DreadSpider () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Body = 11;
			BaseSoundID = 1170;

			SetStr( 196, 210 );
			SetDex( 126, 144 );
			SetInt( 302, 309 );

			SetDamage( 5, 17 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Poison, 80 );

			SetResistance( ResistanceType.Physical, 40, 50 );
			SetResistance( ResistanceType.Fire, 20, 30 );
			SetResistance( ResistanceType.Cold, 20, 30 );
			SetResistance( ResistanceType.Poison, 90, 100 );
			SetResistance( ResistanceType.Energy, 20, 30 );

			SetSkill( SkillName.Magery, 73.1, 82.0 );
			SetSkill( SkillName.MagicResist, 45.1, 55.0 );
			SetSkill( SkillName.Tactics, 58.1, 69.0 );
			SetSkill( SkillName.Wrestling, 71.1, 75.0 );

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 36;

			switch ( Utility.Random( 5 ) )
			{
				case 0: PackItem( new GreaterPoisonPotion() ); break;
				case 1: PackItem( new LesserPoisonPotion() ); break;
				case 2: PackItem( new PoisonPotion() ); break;
				case 3: case 4: PackItem( new SpidersSilk( 8 ) ); break;
			}


			if ( Utility.Random( 100 ) == 0 )
				PackItem( new RareBlueCarpet( PieceType.SouthEdge ));
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
		}

		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override Poison HitPoison{ get{ return Poison.Lethal; } }
		public override int TreasureMapLevel{ get{ return 3; } }

		public DreadSpider( Serial serial ) : base( serial )
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

			if ( BaseSoundID == 263 )
				BaseSoundID = 1170;
		}
	}
}