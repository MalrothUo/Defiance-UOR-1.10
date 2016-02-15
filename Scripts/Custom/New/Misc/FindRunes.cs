using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Regions;
using Server.Commands;

namespace Server.Misc
{
	public class FindRunes
	{
		public static void Initialize()
		{
			CommandSystem.Register( "FindRunes", AccessLevel.Administrator, new CommandEventHandler( FindRunes_OnCommand ) );
			CommandSystem.Register( "DeleteRunes", AccessLevel.Administrator, new CommandEventHandler( DeleteRunes_OnCommand ) );
		}

		[Usage( "FindRunes" )]
		[Description( "Lists runes (or runebooks with runes) to green acres." )]
		public static void FindRunes_OnCommand( CommandEventArgs e )
		{
			foreach ( Item item in World.Items.Values )
			{
				if ( item is RecallRune )
				{
					RecallRune rune = (RecallRune)item;

					if ( rune.Marked && rune.TargetMap != null && IsBad( rune.Target, rune.TargetMap ) )
					{
						object root = item.RootParent;

						if ( root is Mobile )
						{
							if ( ((Mobile)root).AccessLevel < AccessLevel.GameMaster )
								e.Mobile.SendMessage( "Rune: '{4}' {0} [{1}]: {2} ({3})", item.GetWorldLocation(), item.Map, root.GetType().Name, ((Mobile)root).Name, rune.Description );
						}
						else
						{
							e.Mobile.SendMessage( "Rune: '{3}' {0} [{1}]: {2}", item.GetWorldLocation(), item.Map, root==null ? "(null)" : root.GetType().Name, rune.Description );
						}
					}
				}
				else if ( item is Runebook )
				{
					Runebook book = (Runebook)item;

					for ( int i = 0; i < book.Entries.Count; ++i )
					{
						RunebookEntry entry = (RunebookEntry)book.Entries[i];

						if ( entry.Map != null && IsBad( entry.Location, entry.Map ) )
						{
							object root = item.RootParent;

							if ( root is Mobile )
							{
								if ( ((Mobile)root).AccessLevel < AccessLevel.GameMaster )
									e.Mobile.SendMessage( "Runebook: '{6}' {0} [{1}]: {2} ({3}) ({4}:{5})", item.GetWorldLocation(), item.Map, root.GetType().Name, ((Mobile)root).Name, i, entry.Description, book.Description );
							}
							else
							{
								e.Mobile.SendMessage( "Runebook: '{5}' {0} [{1}]: {2} ({3}:{4})", item.GetWorldLocation(), item.Map, root==null ? "(null)" : root.GetType().Name, i, entry.Description, book.Description );
							}
						}
					}
				}
			}
		}

		[Usage( "DeleteRunes" )]
		[Description( "Lists runes (or runebooks with runes) to green acres." )]
		public static void DeleteRunes_OnCommand( CommandEventArgs e )
		{
			ArrayList items = new ArrayList( World.Items.Values );
			foreach ( Item item in items )
			{
				if ( item is RecallRune )
				{
					RecallRune rune = (RecallRune)item;

					if ( rune.Marked && rune.TargetMap != null && IsBad( rune.Target, rune.TargetMap ) )
					{
						object root = item.RootParent;

						if ( root is Mobile )
						{
							if ( ((Mobile)root).AccessLevel < AccessLevel.GameMaster )
							{
								item.Delete();
								e.Mobile.SendMessage( "Rune: '{4}' {0} [{1}]: {2} ({3})", item.GetWorldLocation(), item.Map, root.GetType().Name, ((Mobile)root).Name, rune.Description );
							}
						}
						else
						{
							item.Delete();
							e.Mobile.SendMessage( "Rune: '{3}' {0} [{1}]: {2}", item.GetWorldLocation(), item.Map, root==null ? "(null)" : root.GetType().Name, rune.Description );
						}
					}
				}
				else if ( item is Runebook )
				{
					Runebook book = (Runebook)item;
					List<RunebookEntry> entries = book.Entries;
					for ( int i = 0; i < entries.Count; ++i )
					{
						RunebookEntry entry = entries[i];

						if ( entry.Map != null && IsBad( entry.Location, entry.Map ) )
						{
							object root = item.RootParent;

							if ( root is Mobile )
							{
								if ( ((Mobile)root).AccessLevel < AccessLevel.GameMaster )
								{
									entries.RemoveAt( i );
									e.Mobile.SendMessage( "Runebook: '{6}' {0} [{1}]: {2} ({3}) ({4}:{5})", item.GetWorldLocation(), item.Map, root.GetType().Name, ((Mobile)root).Name, i, entry.Description, book.Description );
								}
							}
							else
							{
								entries.RemoveAt( i );
								e.Mobile.SendMessage( "Runebook: '{5}' {0} [{1}]: {2} ({3}:{4})", item.GetWorldLocation(), item.Map, root==null ? "(null)" : root.GetType().Name, i, entry.Description, book.Description );
							}
						}
					}
				}
			}
		}

		public static bool IsBad( Point3D loc, Map map )
		{
			if ( loc.X < 0 || loc.Y < 0 || loc.X >= map.Width || loc.Y >= map.Height )
			{
				return true;
			}
			else if ( map == Map.Trammel || map == Map.Ilshenar || map == Map.Malas )
			{
				return true;
			}
			else if ( map == Map.Felucca || map == Map.Trammel )
			{
				if ( loc.X >= 5120 && loc.Y >= 0 && loc.X <= 6143 && loc.Y <= 2304 )
				{
					Region r = Region.Find( loc, map );

					if ( !(r is DungeonRegion) )
						return true;
				}
			}

			return false;
		}
	}
}