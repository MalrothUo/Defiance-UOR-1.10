using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Xanthos.Evo
{
	public class HiryuEvoDust : BaseEvoDust
	{
		[Constructable]
		public HiryuEvoDust() : this( 1 )
		{
		}

		[Constructable]
		public HiryuEvoDust( int amount ) : base( amount )
		{
			Amount = amount;
			Name = "mutant dust";
			Hue = 1175;
		}

		public HiryuEvoDust( Serial serial ) : base ( serial )
		{
		}

		public override BaseEvoDust NewDust()
		{
			return new HiryuEvoDust();
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
		}
	}
}