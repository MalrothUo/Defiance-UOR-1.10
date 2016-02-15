using System;
using System.Collections.Generic;
using Server;
using Server.Engines.Craft;
using Server.Items;

namespace Server.Engines.BulkOrders
{
	public class SmallTailorBOD : SmallBOD
	{
		public static double[] m_TailoringMaterialChances = new double[]
			{
				0.857421875, // None
				0.125000000, // Spined
				0.015625000, // Horned
				0.001953125  // Barbed
			};

		public override int ComputeFame()
		{
			int bonus = 0;

			if ( RequireExceptional )
				bonus += 20;

			if ( Material >= BulkMaterialType.DullCopper && Material <= BulkMaterialType.Valorite )
				bonus += 20 * (1 + (int)(Material - BulkMaterialType.DullCopper));
			else if ( Material >= BulkMaterialType.Spined && Material <= BulkMaterialType.Barbed )
				bonus += 40 * (1 + (int)(Material - BulkMaterialType.Spined));

			return 10 + Utility.Random( bonus );
		}

		public override int ComputeGold()
		{
			int bonus = 0;

			if ( RequireExceptional )
				bonus += 500;

			if ( Material >= BulkMaterialType.DullCopper && Material <= BulkMaterialType.Valorite )
				bonus += 250 * (1 + (int)(Material - BulkMaterialType.DullCopper));
			else if ( Material >= BulkMaterialType.Spined && Material <= BulkMaterialType.Barbed )
				bonus += 500 * (1 + (int)(Material - BulkMaterialType.Spined));

			return 750 + Utility.Random( bonus );
		}

		private enum Mat
		{
			Cloth,
			Plain,
			Spined,
			Horned,
			Barbed
		}

		public Item MakeCloth( int hue1, int hue2, int hue3, int hue4 )
		{
			int hue;

			switch ( Utility.Random( 4 ) )
			{
				default:
				case 0: hue = hue1+0; break;
				case 1: hue = hue2+0; break;
				case 2: hue = hue3+0; break;
				case 3: hue = hue4+0; break;
			}

			UncutCloth v = new UncutCloth( 1 );

			v.Hue = hue;

			return v;
		}

		public override List<Item> ComputeRewards( bool full )
		{
			List<Item> list = new List<Item>();

			bool cloth1 = false, cloth2 = false;
			bool cloth3 = false, cloth4 = false;
			bool cloth5 = false, sandals = false;
			bool ps5 = false, ps10 = false;
			bool smallHides = false, mediumHides = false;

			Mat mat;

			switch ( Material )
			{
				default: mat = ( Type.IsSubclassOf( typeof( BaseArmor ) ) || Type.IsSubclassOf( typeof( BaseShoes ) ) ) ? Mat.Plain : Mat.Cloth; break;
				case BulkMaterialType.Spined: mat = Mat.Spined; break;
				case BulkMaterialType.Horned: mat = Mat.Horned; break;
				case BulkMaterialType.Barbed: mat = Mat.Barbed; break;
			}

			if ( Core.AOS )
			{
				if ( RequireExceptional )
				{
					if ( AmountMax >= 20 )
					{
						cloth4 = ( mat >= Mat.Cloth && mat <= Mat.Plain );
						cloth5 = ( mat >= Mat.Spined && mat <= Mat.Horned );
						sandals = ( mat >= Mat.Cloth && mat <= Mat.Horned );
						ps5 = ( mat >= Mat.Spined && mat <= Mat.Horned );
						ps10 = ( mat == Mat.Barbed );
						smallHides = ( mat == Mat.Barbed );
						mediumHides = ( mat == Mat.Barbed );
					}
					else
					{
						cloth3 = ( mat >= Mat.Cloth && mat <= Mat.Plain );
						cloth4 = ( mat == Mat.Spined );
						cloth5 = ( mat >= Mat.Horned && mat <= Mat.Barbed );
						sandals = ( mat >= Mat.Spined && mat <= Mat.Barbed );
						ps5 = ( mat >= Mat.Horned && mat <= Mat.Barbed );
					}
				}
				else
				{
					if ( AmountMax >= 20 )
					{
						cloth2 = ( mat >= Mat.Cloth && mat <= Mat.Plain );
						cloth3 = ( mat == Mat.Spined );
						cloth4 = ( mat == Mat.Horned );
						cloth5 = ( mat == Mat.Barbed );
						sandals = ( mat >= Mat.Horned && mat <= Mat.Barbed );
						ps5 = ( mat == Mat.Barbed );
					}
					else
					{
						cloth1 = ( mat >= Mat.Cloth && mat <= Mat.Plain );
						cloth2 = ( mat == Mat.Spined );
						cloth3 = ( mat == Mat.Horned );
						cloth4 = ( mat == Mat.Barbed );
						sandals = ( mat == Mat.Barbed );
					}
				}
			}
			else
			{
				if ( RequireExceptional )
				{
					if ( AmountMax >= 20 )
					{
						cloth4 = ( mat >= Mat.Cloth && mat <= Mat.Horned );
						cloth5 = ( mat == Mat.Barbed );
						sandals = ( mat >= Mat.Spined && mat <= Mat.Barbed );
						ps5 = ( mat == Mat.Barbed );
					}
					else
					{
						cloth3 = ( mat >= Mat.Cloth && mat <= Mat.Plain );
						cloth4 = ( mat >= Mat.Spined && mat <= Mat.Barbed );
						sandals = ( mat >= Mat.Horned && mat <= Mat.Barbed );
					}
				}
				else
				{
					if ( AmountMax >= 20 )
					{
						cloth2 = ( mat >= Mat.Cloth && mat <= Mat.Plain );
						cloth3 = ( mat == Mat.Spined );
						cloth4 = ( mat >= Mat.Horned && mat <= Mat.Barbed );
						sandals = ( mat == Mat.Barbed );
					}
					else
					{
						cloth1 = ( mat >= Mat.Cloth && mat <= Mat.Plain );
						cloth2 = ( mat == Mat.Spined );
						cloth3 = ( mat == Mat.Horned );
						cloth4 = ( mat == Mat.Barbed );
					}
				}
			}

			if ( cloth1 )
				list.Add( MakeCloth( 0x557, 0x488, 0x4F4, 0x48C ) );

			if ( cloth2 )
				list.Add( MakeCloth( 0x55D, 0x48B, 0x4FB, 0x492 ) );

			if ( cloth3 )
				list.Add( MakeCloth( 0x490, 0x48E, 0x48D, 0x495 ) );

			if ( cloth4 )
				list.Add( MakeCloth( 0x455, 0x501, 0x487, 0x494 ) );

			if ( cloth5 )
				list.Add( MakeCloth( 0x4F2, 0x4AA, 0x4AC, 0x47E ) );

			if ( sandals )
				list.Add( new Sandals( Utility.RandomList( 0x490, 0x487, 0x4F2, 0x501, 0x48B, 0x4F4, 0x4FB, 0x494, 0x495, 0x48F, 0x48E, 0x455 ) ) );

			if ( ps5 )
				list.Add( new PowerScroll( SkillName.Tailoring, 105 ) );

			if ( ps10 )
				list.Add( new PowerScroll( SkillName.Tailoring, 110 ) );

			if ( smallHides )
			{
				if ( Utility.RandomBool() )
					list.Add( new SmallStretchedHideEastDeed() );
				else
					list.Add( new SmallStretchedHideSouthDeed() );
			}

			if ( mediumHides )
			{
				if ( Utility.RandomBool() )
					list.Add( new MediumStretchedHideEastDeed() );
				else
					list.Add( new MediumStretchedHideSouthDeed() );
			}

			return list;
		}

		public static SmallTailorBOD CreateRandomFor( Mobile m )
		{
			SmallBulkEntry[] entries;
			bool useMaterials;

			double theirSkill = m.Skills[SkillName.Tailoring].Base;
			if ( useMaterials = Utility.RandomBool() && theirSkill >= 6.2 ) // Ugly, but the easiest leather BOD is Leather Cap which requires at least 6.2 skill.
				entries = SmallBulkEntry.TailorLeather;
			else
				entries = SmallBulkEntry.TailorCloth;

			if ( entries.Length > 0 )
			{
				int amountMax;

				if ( theirSkill >= 70.1 )
					amountMax = Utility.RandomList( 10, 15, 20, 20 );
				else if ( theirSkill >= 50.1 )
					amountMax = Utility.RandomList( 10, 15, 15, 20 );
				else
					amountMax = Utility.RandomList( 10, 10, 15, 20 );

				BulkMaterialType material = BulkMaterialType.None;

				if ( useMaterials && theirSkill >= 70.1 )
				{
					for ( int i = 0; i < 20; ++i )
					{
						BulkMaterialType check = GetRandomMaterial( BulkMaterialType.Spined, m_TailoringMaterialChances );
						double skillReq = 0.0;

						switch ( check )
						{
							case BulkMaterialType.DullCopper: skillReq = 65.0; break;
							case BulkMaterialType.Bronze: skillReq = 80.0; break;
							case BulkMaterialType.Gold: skillReq = 85.0; break;
							case BulkMaterialType.Agapite: skillReq = 90.0; break;
							case BulkMaterialType.Verite: skillReq = 95.0; break;
							case BulkMaterialType.Valorite: skillReq = 100.0; break;
							case BulkMaterialType.Spined: skillReq = 65.0; break;
							case BulkMaterialType.Horned: skillReq = 80.0; break;
							case BulkMaterialType.Barbed: skillReq = 99.0; break;
						}

						if ( theirSkill >= skillReq )
						{
							material = check;
							break;
						}
					}
				}

				double excChance = 0.0;

				if ( theirSkill >= 70.1 )
					excChance = (theirSkill + 80.0) / 200.0;

				bool reqExceptional = ( excChance > Utility.RandomDouble() );


				CraftSystem system = DefTailoring.CraftSystem;

				List<SmallBulkEntry> validEntries = new List<SmallBulkEntry>();

				for ( int i = 0; i < entries.Length; ++i )
				{
					CraftItem item = system.CraftItems.SearchFor( entries[i].Type );

					if ( item != null )
					{
						bool allRequiredSkills = true;
						double chance = item.GetSuccessChance( m, null, system, false, ref allRequiredSkills );

						if ( allRequiredSkills && chance >= 0.0 )
						{
							if ( reqExceptional )
								chance = item.GetExceptionalChance( system, chance, m );

							if ( chance > 0.0 )
								validEntries.Add( entries[i] );
							}
						}
					}

				if ( validEntries.Count > 0 )
				{
					SmallBulkEntry entry = validEntries[Utility.Random( validEntries.Count )];
					return new SmallTailorBOD( entry, material, amountMax, reqExceptional, m );
				}
			}

			return null;
		}

		private SmallTailorBOD( SmallBulkEntry entry, BulkMaterialType material, int amountMax, bool reqExceptional, Mobile m )
		{
			this.Hue = 0x483;
			this.AmountMax = amountMax;
			this.Type = entry.Type;
			this.Number = entry.Number;
			this.Graphic = entry.Graphic;
			this.RequireExceptional = reqExceptional;
			this.Material = material;
			this.Owner = m;
		}

		[Constructable]
		public SmallTailorBOD()
		{
			SmallBulkEntry[] entries;
			bool useMaterials;

			if ( useMaterials = Utility.RandomBool() )
				entries = SmallBulkEntry.TailorLeather;
			else
				entries = SmallBulkEntry.TailorCloth;

			if ( entries.Length > 0 )
			{
				int hue = 0x483;
				int amountMax = Utility.RandomList( 10, 15, 20 );

				BulkMaterialType material;

				if ( useMaterials )
					material = GetRandomMaterial( BulkMaterialType.Spined, m_TailoringMaterialChances );
				else
					material = BulkMaterialType.None;

				bool reqExceptional = Utility.RandomBool() || (material == BulkMaterialType.None);

				SmallBulkEntry entry = entries[Utility.Random( entries.Length )];

				this.Hue = hue;
				this.AmountMax = amountMax;
				this.Type = entry.Type;
				this.Number = entry.Number;
				this.Graphic = entry.Graphic;
				this.RequireExceptional = reqExceptional;
				this.Material = material;
			}
		}

		public SmallTailorBOD( int amountCur, int amountMax, Type type, int number, int graphic, bool reqExceptional, BulkMaterialType mat, Mobile m )
		{
			this.Hue = 0x483;
			this.AmountMax = amountMax;
			this.AmountCur = amountCur;
			this.Type = type;
			this.Number = number;
			this.Graphic = graphic;
			this.RequireExceptional = reqExceptional;
			this.Material = mat;
		}

		public SmallTailorBOD( Serial serial ) : base( serial )
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