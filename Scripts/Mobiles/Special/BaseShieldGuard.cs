using System;
using Server;
using Server.Misc;
using Server.Items;
using Server.Guilds;

namespace Server.Mobiles
{
	public abstract class BaseShieldGuard : BaseCreature
	{
		public BaseShieldGuard() : base( AIType.AI_Melee, FightMode.Aggressor, 14, 1, 0.8, 1.6 )
		{
			InitStats( 1000, 1000, 1000 );
			Title = "the guard";

			SpeechHue = Utility.RandomDyedHue();

			Hue = Utility.RandomSkinHue();

			if ( Female = Utility.RandomBool() )
			{
				Body = 0x191;
				Name = NameList.RandomName( "female" );

				AddItem( new FemalePlateChest() );
				AddItem( new PlateArms() );
				AddItem( new PlateLegs() );

				switch( Utility.Random( 2 ) )
				{
					case 0: AddItem( new Doublet( Utility.RandomNondyedHue() ) ); break;
					case 1: AddItem( new BodySash( Utility.RandomNondyedHue() ) ); break;
				}

				switch( Utility.Random( 2 ) )
				{
					case 0: AddItem( new Skirt( Utility.RandomNondyedHue() ) ); break;
					case 1: AddItem( new Kilt( Utility.RandomNondyedHue() ) ); break;
				}
			}
			else
			{
				Body = 0x190;
				Name = NameList.RandomName( "male" );

				AddItem( new PlateChest() );
				AddItem( new PlateArms() );
				AddItem( new PlateLegs() );

				switch( Utility.Random( 3 ) )
				{
					case 0: AddItem( new Doublet( Utility.RandomNondyedHue() ) ); break;
					case 1: AddItem( new Tunic( Utility.RandomNondyedHue() ) ); break;
					case 2: AddItem( new BodySash( Utility.RandomNondyedHue() ) ); break;
				}
			}

			Utility.AssignRandomHair( this );
			if( Utility.RandomBool() )
				Utility.AssignRandomFacialHair( this, HairHue );

			VikingSword weapon = new VikingSword();
			weapon.Movable = false;
			AddItem( weapon );

			BaseShield shield = Shield;
			shield.Movable = false;
			AddItem( shield );

			PackGold( 250, 300 );

			Skills[SkillName.Anatomy].Base = 120.0;
			Skills[SkillName.Tactics].Base = 120.0;
			Skills[SkillName.Swords].Base = 120.0;
			Skills[SkillName.MagicResist].Base = 120.0;
			Skills[SkillName.DetectHidden].Base = 100.0;
		}

		public abstract int Keyword{ get; }
		public abstract BaseShield Shield{ get; }
		public abstract int SignupNumber{ get; }
		public abstract GuildType Type{ get; }

		public override bool HandlesOnSpeech( Mobile from )
		{
			if ( from.InRange( this.Location, 2 ) )
				return true;

			return base.HandlesOnSpeech( from );
		}

		public override void OnSpeech( SpeechEventArgs e )
		{
			if ( !e.Handled && e.HasKeyword( Keyword ) && e.Mobile.InRange( this.Location, 2 ) )
			{
				e.Handled = true;

				Mobile from = e.Mobile;
				Guild g = from.Guild as Guild;

				if ( g == null || g.Type != Type )
				{
					Say( SignupNumber );
				}
				else
				{
					Container pack = from.Backpack;
					BaseShield shield = Shield;
					Item twoHanded = from.FindItemOnLayer( Layer.TwoHanded );

					if ( (pack != null && pack.FindItemByType( shield.GetType() ) != null) || ( twoHanded != null && shield.GetType().IsAssignableFrom( twoHanded.GetType() ) ) )
					{
						Say( 1007110 ); // Why dost thou ask about virtue guards when thou art one?
						shield.Delete();
					}
					else if ( from.PlaceInBackpack( shield ) )
					{
						Say( Utility.Random( 1007101, 5 ) );
						Say( 1007139 ); // I see you are in need of our shield, Here you go.
						from.AddToBackpack( shield );
					}
					else
					{
						from.SendLocalizedMessage( 502868 ); // Your backpack is too full.
						shield.Delete();
					}
				}
			}

			base.OnSpeech( e );
		}

		public BaseShieldGuard( Serial serial ) : base( serial )
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
/*
		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			Head head = dropped as Head;
			if ( head != null && head.Owner != null && head.Owner is PlayerMobile )
			{
				PlayerMobile victim = head.Owner as PlayerMobile;
				int bounty = BountyTable.Bounty( victim );
				Say( 500670 ); // Ah, a head! Let me check to see if there is a bounty on this.
				if ( bounty <= 0 )
				{
					Say( 1042854, victim.Name );
				}
				else
				{
					if ( Banker.Deposit( from, bounty ) )
					{
						BountyTable.Remove( victim );
						Say( 1042855, String.Format( "{0}\t{1}", victim.Name, bounty ) );
					}
					else
					{
						Say( "There is bounty on this head, but thy bank box is too full to get it!" );
						return false;
					}
				}
				head.Delete();
			}
			return false;
		}
*/
	}
}