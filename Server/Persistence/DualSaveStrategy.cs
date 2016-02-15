/***************************************************************************
 *                            DualSaveStrategy.cs
 *                            -------------------
 *   begin                : May 1, 2002
 *   copyright            : (C) The RunUO Software Team
 *   email                : info@runuo.com
 *
 *   $Id: DualSaveStrategy.cs 4 2006-06-15 04:28:39Z mark $
 *
 ***************************************************************************/

/***************************************************************************
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Diagnostics;

using Server;
using Server.Guilds;

namespace Server {
	public sealed class DualSaveStrategy : StandardSaveStrategy {
		public override string Name {
			get { return "Dual"; }
		}

		public DualSaveStrategy() {
		}

		public override void Save( SaveMetrics metrics ) {
			Thread saveThread = new Thread( delegate() {
				SaveItems( metrics );
			} );

			saveThread.Name = "Item Save Subset";
			saveThread.Start();

			SaveMobiles( metrics );
			SaveGuilds( metrics );
			SaveMaps( metrics );

			saveThread.Join();
		}
	}
}