using Server.Gumps;
using Server;
using System;
using System.Collections;

namespace Server.Items
{
	public class CTFScore : Item
	{
		private CTFGame m_Game;

		[Constructable]
		public CTFScore() : base( 0x1e5e )
		{
			this.Movable = false;
			this.Name = "CTF Score board";
		}

		public CTFScore( Serial serial ) : base(serial)
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 );

			writer.Write( m_Game );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
					m_Game = reader.ReadItem() as CTFGame;
					break;
				}
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public CTFGame Game{ get { return m_Game; } set{ m_Game = value; } }

		public override void OnSingleClick( Mobile from )
		{
			base.OnSingleClick(from);
			from.SendMessage("**** CTF Scores ****");
			if ( m_Game == null ) return;
			for (int i=0;i<m_Game.Teams.Count;i++)
			{
				CTFTeam team = (CTFTeam)m_Game.Teams[i];
				from.SendMessage(team.Name + ": " + team.Points.ToString());
			}
		}
	}
}