using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Misc;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;
using Server.Regions;
using Server.Items;

namespace Server.Gumps
{
	public class ReportMurdererGump : Gump
	{
		private int m_Idx;
		private List<Mobile> m_Killers;
		private Mobile m_Victum;
		private Point3D m_Location;
		private Map m_Map;

		public static void Initialize()
		{
			AggressorInfo.ExpireDelay = TimeSpan.FromMinutes( 5.0 );

			EventSink.PlayerDeath += new PlayerDeathEventHandler( EventSink_PlayerDeath );
		}

		public static void EventSink_PlayerDeath( PlayerDeathEventArgs e )
		{
			Mobile m = e.Mobile;

			List<Mobile> killers = new List<Mobile>();
			List<Mobile> toGive  = new List<Mobile>();

			foreach ( AggressorInfo ai in m.Aggressors )
			{
				CustomRegion region = Region.Find( m.Location, m.Map ) as CustomRegion;

				bool nocountsregion = region != null && region.Controller.NoMurderCounts;

				if ( ai.Attacker.Player && ai.CanReportMurder && !ai.Reported && !nocountsregion )
				{
					killers.Add( ai.Attacker );
					ai.Reported = true;
				}
				if ( ai.Attacker.Player && (DateTime.Now - ai.LastCombatTime) < TimeSpan.FromSeconds( 30.0 ) && !toGive.Contains( ai.Attacker ) )
					toGive.Add( ai.Attacker );
			}

			foreach ( AggressorInfo ai in m.Aggressed )
			{
				if ( ai.Defender.Player && (DateTime.Now - ai.LastCombatTime) < TimeSpan.FromSeconds( 30.0 ) && !toGive.Contains( ai.Defender ) )
					toGive.Add( ai.Defender );
			}

			foreach ( Mobile g in toGive )
			{
				int n = Notoriety.Compute( g, m );

				int theirKarma = m.Karma, ourKarma = g.Karma;
				bool innocent = ( n == Notoriety.Innocent );
				bool criminal = ( n == Notoriety.Criminal || n == Notoriety.Murderer );

				int fameAward = m.Fame / 200;
				int karmaAward = 0;

				if ( innocent )
					karmaAward = ( ourKarma > -2500 ? -850 : -110 - (m.Karma / 100) );
				else if ( criminal )
					karmaAward = 50;

				Titles.AwardFame( g, fameAward, false );
				Titles.AwardKarma( g, karmaAward, true );
			}

			if ( m is PlayerMobile && ((PlayerMobile)m).NpcGuild == NpcGuild.ThievesGuild )
				return;

			if ( killers.Count > 0 )
				new GumpTimer( m, killers, m.Location, m.Map ).Start();

			/*
			TODO:  	Check entire combatant system and see if the
				cobatant lists should be handled a different
				way, and  change it accordingly.  This is a
				small-scope patch to prevent an exploit.
			*/

			for ( int i = m.Aggressors.Count - 1; i >= 0; --i )
				m.Aggressors.RemoveAt ( i );
		}

		private class GumpTimer : Timer
		{
			private Mobile m_Victim;
			private List<Mobile> m_Killers;
			private Point3D m_Location;
			private Map m_Map;

			public GumpTimer( Mobile victim, List<Mobile> killers, Point3D loc, Map map ) : base( TimeSpan.FromSeconds( 4.0 ) )
			{
				m_Victim = victim;
				m_Killers = killers;
				m_Location = loc;
				m_Map = map;
			}

			protected override void OnTick()
			{
				m_Victim.SendGump( new ReportMurdererGump( m_Victim, m_Killers, m_Location, m_Map ) );
			}
		}

		public ReportMurdererGump( Mobile victum, List<Mobile> killers, Point3D loc, Map map ) : this( victum, killers, loc, map, 0 )
		{
		}

		private ReportMurdererGump( Mobile victum, List<Mobile> killers, Point3D loc, Map map, int idx ) : base( 0, 0 )
		{
			m_Killers = killers;
			m_Victum = victum;
			m_Location = loc;
			m_Map = map;
			m_Idx = idx;
			BuildGump();
		}

		private void BuildGump()
		{
			AddBackground( 265, 205, 320, 290, 5054 );
			Closable = false;
			Resizable = false;

			AddPage( 0 );

			AddImageTiled( 225, 175, 50, 45, 0xCE );   //Top left corner
			AddImageTiled( 267, 175, 315, 44, 0xC9 );  //Top bar
			AddImageTiled( 582, 175, 43, 45, 0xCF );   //Top right corner
			AddImageTiled( 225, 219, 44, 270, 0xCA );  //Left side
			AddImageTiled( 582, 219, 44, 270, 0xCB );  //Right side
			AddImageTiled( 225, 489, 44, 43, 0xCC );   //Lower left corner
			AddImageTiled( 267, 489, 315, 43, 0xE9 );  //Lower Bar
			AddImageTiled( 582, 489, 43, 43, 0xCD );   //Lower right corner

			AddPage( 1 );

			AddHtml( 260, 234, 300, 140, ((Mobile)m_Killers[m_Idx]).Name, false, false ); // Player's Name
			AddHtmlLocalized( 260, 254, 300, 140, 1049066, false, false ); // Would you like to report...

			AddButton( 260, 300, 0xFA5, 0xFA7, 1, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 300, 300, 300, 50, 1046362, false, false ); // Yes

			AddButton( 360, 300, 0xFA5, 0xFA7, 2, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 400, 300, 300, 50, 1046363, false, false ); // No
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile from = state.Mobile;

			switch ( info.ButtonID )
			{
				case 1:
				{
					Mobile killer = m_Killers[m_Idx];
					if ( killer != null && !killer.Deleted )
					{
						Region region = Region.Find( m_Location, m_Map );
						CustomRegion cregion = region as CustomRegion;
						GuardedRegion gregion = region as GuardedRegion;

						if( region is DungeonRegion || (gregion != null && !gregion.IsDisabled()) || ( cregion != null && cregion.Controller.IsDungeonRegion ) )
							killer.ShortTermMurders++;

						killer.Kills++;
						//killer.ShortTermMurders++;

						if ( killer is PlayerMobile )
							((PlayerMobile)killer).ResetKillTime();

						killer.SendLocalizedMessage( 1049067 );//You have been reported for murder!

						if ( killer.Kills == 5 )
							killer.SendLocalizedMessage( 502134 );//You are now known as a murderer!
						else if ( SkillHandlers.Stealing.SuspendOnMurder && killer.Kills == 1 && killer is PlayerMobile && ((PlayerMobile)killer).NpcGuild == NpcGuild.ThievesGuild )
							killer.SendLocalizedMessage( 501562 ); // You have been suspended by the Thieves Guild.
					}
					break;
				}
				case 2:
				{
					break;
				}
			}

			m_Idx++;
			if ( m_Idx < m_Killers.Count )
				from.SendGump( new ReportMurdererGump( from, m_Killers, m_Location, m_Map, m_Idx ) );
		}
	}
}